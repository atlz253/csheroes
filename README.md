# Герои зачета и сессии

Игра, вдохновленная серией игр «Heroes of Might and Magic». Курсовая работа второго курса по дисциплине «Разработка на ЯВУ».

Desktop-версию можно скачать на [странице релизов](https://github.com/atlz253/csheroes/releases).

![Скриншот игры](https://raw.githubusercontent.com/atlz253/csheroes/master/screenshots/1.png)

## Web-версия

В репозитории есть web-клиент на Blazor WebAssembly с рендерингом через Canvas.

Для запуска нужен .NET SDK 8:

```powershell
dotnet run --project CSHeroes.Web\CSHeroes.Web.csproj
```

Web-клиент использует отдельный проект `CSHeroes.Core`, который подключает оригинальные исходники игровой модели и отдает платформонезависимые snapshot-данные для Canvas-рендера. Карты загружаются из `wwwroot/assets/maps`, а сохранения в браузере хранятся в `localStorage`.
