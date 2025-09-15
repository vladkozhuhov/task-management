# Система управления задачами (TaskManagement)

Бэкенд для простой системы управления задачами (аналог Jira), разработанный с использованием Clean Architecture, .NET 8, ASP.NET Core и Entity Framework Core.

## Используемые технологии

- .NET 8, ASP.NET Core
- Entity Framework Core с PostgreSQL
- JWT для авторизации
- CQRS с использованием MediatR
- AutoMapper для маппинга объектов
- Swagger для документации API
- Serilog для логирования
- Docker и Docker Compose для контейнеризации
- Unit-тесты с использованием xUnit, Moq и FluentAssertions

## Запуск проекта

### Требования

- Docker и Docker Compose
- .NET SDK 8.0 (опционально, для разработки)

### Запуск в Docker

1. Клонировать репозиторий
```bash
git clone <url_репозитория>
cd TaskManagement
```

2. Запустить приложение с помощью Docker Compose
```bash
docker-compose up -d
```

3. API будет доступно по адресу:
   - HTTP: http://localhost:5050
   - Swagger: http://localhost:5050/swagger

## Проверка функционала

### Аутентификация

1. Регистрация пользователя
```http
POST /api/auth/register
Content-Type: application/json

{
  "userName": "testuser",
  "email": "test@example.com",
  "password": "Password123!"
}
```

2. Вход в систему
```http
POST /api/auth/login
Content-Type: application/json

{
  "email": "test@example.com",
  "password": "Password123!"
}
```

### Работа с задачами

1. Создание задачи
```http
POST /api/tasks
Authorization: Bearer <ваш_токен>
Content-Type: application/json

{
  "title": "Реализовать API",
  "description": "Создать REST API для управления задачами",
  "priority": "High",
  "dueDate": "2023-12-31T12:00:00Z"
}
```

2. Получение списка задач
```http
GET /api/tasks
Authorization: Bearer <ваш_токен>
```

3. Фильтрация задач по статусу и приоритету
```http
GET /api/tasks?status=InProgress&priority=High
Authorization: Bearer <ваш_токен>
```

4. Получение конкретной задачи
```http
GET /api/tasks/{id}
Authorization: Bearer <ваш_токен>
```

5. Обновление задачи
```http
PUT /api/tasks/{id}
Authorization: Bearer <ваш_токен>
Content-Type: application/json

{
  "title": "Обновленное название",
  "status": "InProgress"
}
```

6. Удаление задачи
```http
DELETE /api/tasks/{id}
Authorization: Bearer <ваш_токен>
```

7. Создание связи между задачами
```http
POST /api/tasks/relations
Authorization: Bearer <ваш_токен>
Content-Type: application/json

{
  "sourceTaskId": "task-id-1",
  "targetTaskId": "task-id-2",
  "relationType": "BlockedBy"
}
```

## Архитектура проекта

Проект реализован с использованием принципов Clean Architecture:

- **Domain** - содержит бизнес-модели, перечисления и интерфейсы репозиториев
- **Application** - содержит бизнес-логику, CQRS команды/запросы и DTO
- **Infrastructure** - содержит реализации интерфейсов, работу с БД, JWT и другие внешние сервисы
- **WebApi** - содержит контроллеры API и конфигурацию приложения
- **Tests** - содержит юнит-тесты для бизнес-логики

## Дополнительная информация

Для полного ознакомления с API используйте Swagger документацию, доступную по адресу http://localhost:5050/swagger после запуска приложения.