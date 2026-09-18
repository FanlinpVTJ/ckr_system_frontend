# FeaturedClicker

Тестовый Unity-проект с тремя вкладками: кликер, прогноз погоды и список пород собак. Проект построен вокруг Zenject, MVP-подхода и единой последовательной очереди HTTP-запросов.

## Технологии

- Unity `6000.1.17f1` (Unity 6)
- Zenject для композиции зависимостей
- UniTask для асинхронных операций
- UnityWebRequest для HTTP
- Newtonsoft JSON для сетевой десериализации
- DOTween для UI-анимаций
- UI Particle Effect for UGUI для эффектов нажатия
- SmartScroll и PoolManager для переиспользования UI-элементов
- WindowsManager для окон и попапов

## Структура

Основной код расположен в `Assets/GameAssets/Scripts`. Все пространства имён начинаются с `FeaturedClicker` и повторяют логическую структуру модулей.

| Путь | Namespace | Назначение |
| --- | --- | --- |
| `Clicker` | `FeaturedClicker.Clicker` | Игровая логика кликера, MVP и эффекты валюты |
| `Network/Network` | `FeaturedClicker.Network` | HTTP-контракты, JSON, очередь запросов и UnityWebRequest-исполнитель |
| `Network/Features/Dogs` | `FeaturedClicker.Network.Features.Dogs` | HTTP-клиент и DTO Dog API |
| `Network/Features/Weather` | `FeaturedClicker.Network.Features.Weather` | HTTP-клиент и DTO weather.gov |
| `Network/UI` | `FeaturedClicker.Network.UI` | Окно сетевой ошибки |
| `Weather` | `FeaturedClicker.Weather` | MVP вкладки прогноза и сопоставление иконок |
| `Dogs` | `FeaturedClicker.Dogs` | MVP вкладки пород и окно деталей |
| `Save` | `FeaturedClicker.Save` | Абстрактные сохранения и адаптер значений |
| `Sound` | `FeaturedClicker.Sound` | Конфигурируемое воспроизведение звука |
| `UI` | `FeaturedClicker.UI` | Общие UI-компоненты |

## Архитектурный подход

UI-вкладки используют MVP:

- `View` хранит ссылки на Unity UI, публикует пользовательские события и отображает состояние.
- `Presenter` подписывается на события View, вызывает сервисы и координирует асинхронные операции.
- Модели и сервисы не зависят от Unity UI.

Глобальные зависимости создаются через Zenject в `ProjectContext`. Локальные presenter-ы вкладок регистрируются scene installer-ами. Singleton-паттерн в прикладном коде не используется.

## Сетевой модуль

`RequestQueueService` реализует FIFO-очередь. В один момент времени исполняется только один `UnityWebRequest`; следующий начинает работу после завершения, отмены или ошибки предыдущего.

Контракты модуля:

- `HttpRequest<TResponse>` описывает URL, HTTP-метод, заголовки, тело и scope запроса.
- `HttpResponse<TResponse>` возвращает результат либо ошибку выполнения.
- `IJsonConverter` изолирует JSON-десериализацию от очереди.
- `IHttpRequestExecutor` изолирует `UnityWebRequest` от логики очереди.
- `RequestScopeType` позволяет отменять запросы конкретной вкладки.

Отмена работает для двух состояний: запрос удаляется из очереди, если ещё не стартовал, или прерывается, если уже выполняется. Ошибки сети и HTTP отображаются через `INetworkErrorWindowService`.

Endpoint-адреса хранятся в ScriptableObject-конфигах:

- `WeatherEndpointConfig`
- `DogEndpointConfig`

Конфиги и API-клиенты регистрируются через `WeatherNetworkInstaller` и `DogNetworkInstaller` в `ProjectContext`.

## Кликер

`ClickerService` управляет игровой логикой:

- ручное получение валюты расходует энергию;
- автоматический сбор выполняется раз в три секунды, пока открыта вкладка кликера;
- ручной клик сбрасывает таймер автоматического сбора;
- энергия восстанавливается настраиваемыми порциями с ограничением максимального значения.

Значения награды, стоимости, интервалов и ссылок на валюты вынесены в `ClickerConfig`. Сами монеты и энергия обслуживаются внешним `ValueSystem`; `ClickerService` не хранит баланс самостоятельно.

`ClickerPresenter` соединяет сервис и View. При успешном сборе View запускает звук, UI Particle и анимацию кнопки, а `CurrencyFlightEffect` создаёт из пула иконку валюты и направляет её к счётчику. Иконка берётся из `ValueData` валюты.

## Сохранения

Модуль сохранений не зависит от `ValueSystem`:

- `ISaveSystem` определяет асинхронное чтение, запись и удаление абстрактных данных.
- `PlayerPrefsSaveSystem` является текущей реализацией хранилища.
- `ISaveParticipant` подключает конкретный тип сохраняемых данных.
- `SaveCoordinator` координирует всех участников.
- `SaveSystemLifecycleHandler` запускает загрузку при инициализации и сохраняет данные при уничтожении DI-контекста.

`ValueSaveParticipant` является отдельным адаптером между сохранениями и `ValueSystem`. Он загружает `ValuesSaveData` из ключа `values` и сохраняет его после каждого изменения значения. При переходе на файл, облако или сервер меняется реализация `ISaveSystem`, а не система валют.

## Звук

`SoundConfig` содержит список `SoundDefinition`: идентификатор, клип, громкость и pitch. `SoundManager` реализует `ISoundManager` и воспроизводит звук по идентификатору. `ButtonSound` позволяет назначить звук абстрактной Unity-кнопке без зависимости от конкретной игровой механики.

## Вкладка погоды

`WeatherPresenter` запускает запрос прогноза сразу при открытии вкладки и затем раз в пять секунд. При скрытии вкладки запросы scope `Weather` отменяются. `WeatherForecastMapper` преобразует DTO weather.gov в UI-модель, а `WeatherIconConfig` сопоставляет код погодного состояния, время суток и Sprite.

## Вкладка пород собак

При открытии `DogBreedsPresenter` запрашивает первые десять пород из Dog API и показывает загрузчик. `DogBreedListDataProvider` хранит список и публикует выбранную породу, `DogBreedScrollView` отображает элементы через SmartScroll.

Выбор породы отменяет предыдущий запрос деталей, скрывает его загрузчик и ставит новый запрос в очередь. После ответа `DogBreedDetailsWindowService` открывает общий popup с названием и описанием. При уходе со вкладки отменяются запросы списка и деталей.

## Unity-настройка

`Assets/Resources/ProjectContext.prefab` должен содержать:

- `NetworkInstaller` и оба feature installer-а API;
- `SaveSystemInstaller`;
- `SoundInstaller`;
- `ValuesInstaller` ScriptableObject;
- installer-ы окон `WindowsManager`, сетевой ошибки и деталей породы.

В `MainScene` должны быть добавлены scene installer-ы вкладок:

- `ClickerSceneInstaller`
- `WeatherSceneInstaller`
- `DogBreedsSceneInstaller`

Ссылки на View, ScriptableObject-конфиги, loaders, Sprite, окна и префабы задаются в Inspector. Префабы не создаются и не изменяются из кода.

## Проверка

Перед сдачей проверьте:

- быстрые переходы между вкладками во время сетевого запроса;
- отмену погодного запроса при уходе с вкладки;
- смену породы до завершения предыдущего запроса;
- ручной и автоматический сбор при достаточной и нулевой энергии;
- сохранение монет и энергии между Play Mode-сессиями;
- сетевую ошибку и повторное открытие вкладок;
- портретное и широкое разрешения UI.

Тесты очереди находятся в `Assets/Tests/Editor/RequestQueueServiceTests.cs`.
