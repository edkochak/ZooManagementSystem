# ZooManagementSystem

## Обзор проекта

**ZooManagementSystem** — это комплексное решение для управления зоопарком, разработанное с использованием C# и .NET. Система построена по принципам Domain-Driven Design (DDD) и обеспечивает полный контроль над всеми аспектами деятельности зоопарка, включая управление животными, вольерами, расписаниями кормления и ведение статистики.

![Zoo Management System](https://via.placeholder.com/800x400?text=Zoo+Management+System)

## 📋 Содержание

- [Функциональные возможности](#функциональные-возможности)
- [Архитектура системы](#архитектура-системы)
- [Структура проекта](#структура-проекта)
- [Технический стек](#технический-стек)
- [Установка и запуск](#установка-и-запуск)
- [Тестирование](#тестирование)
- [Примеры использования API](#примеры-использования-api)
- [Вклад в проект](#вклад-в-проект)
- [Лицензия](#лицензия)

## 🔥 Функциональные возможности

### 🦁 Управление животными
- Регистрация новых животных с указанием вида, имени, даты рождения, пола и предпочитаемой пищи
- Отслеживание перемещений животных между вольерами
- Управление здоровьем и питанием каждого животного

### 🏞️ Управление вольерами
- Создание вольеров различных типов (для хищников, травоядных, птиц, аквариумы)
- Контроль вместимости и заполненности вольеров
- Мониторинг подходящих условий для животных различных видов

### 🍖 Организация кормления
- Составление расписаний кормления для всех животных
- Учет различных типов питания
- Автоматическое напоминание о времени кормления

### 📊 Статистика зоопарка
- Сбор и анализ данных о посетителях
- Мониторинг состояния животных
- Анализ использования ресурсов зоопарка

### 🔄 Перемещение животных
- Организация безопасного перемещения животных между вольерами
- Предотвращение конфликтных ситуаций (например, размещение хищников с травоядными)
- Отслеживание истории перемещений

## 🏗️ Архитектура системы

ZooManagementSystem построен с использованием архитектуры, основанной на Domain-Driven Design (DDD), что обеспечивает чёткое разделение бизнес-логики от инфраструктурных деталей.

### Основные слои системы:

#### 1. Domain Layer
Ядро системы, содержащее бизнес-логику и правила. Включает в себя:
- Сущности (Entities)
- Объекты-значения (Value Objects)
- Доменные события (Domain Events)
- Доменные сервисы (Domain Services)

#### 2. Application Layer
Слой, отвечающий за координацию выполнения задач приложения:
- Сервисы приложения (Application Services)
- Трансферные объекты данных DTO (Data Transfer Objects)
- Интерфейсы сервисов

#### 3. Infrastructure Layer
Слой, содержащий реализации технических аспектов:
- Репозитории (реализации интерфейсов из домена)
- Доступ к базе данных
- Внешние сервисы и API

#### 4. Presentation Layer
Слой, отвечающий за взаимодействие с пользователем:
- Контроллеры API
- Форматы ответов
- Обработка запросов

### Диаграмма взаимодействия компонентов

```
┌─────────────────┐       ┌─────────────────┐       ┌─────────────────┐       ┌─────────────────┐
│                 │       │                 │       │                 │       │                 │
│   Presentation  │ ───▶  │   Application   │ ───▶  │     Domain      │ ◀───  │ Infrastructure  │
│     Layer       │       │     Layer       │       │     Layer       │       │     Layer       │
│                 │       │                 │       │                 │       │                 │
└─────────────────┘       └─────────────────┘       └─────────────────┘       └─────────────────┘
```

## 📁 Структура проекта

```
ZooManagementSystem/
├── Application/
│   ├── DTOs/              # Трансферные объекты данных
│   │   ├── AnimalDto.cs
│   │   ├── EnclosureDto.cs
│   │   └── FeedingScheduleDto.cs
│   ├── Interfaces/        # Интерфейсы сервисов
│   │   ├── IAnimalService.cs
│   │   ├── IEnclosureService.cs
│   │   └── IFeedingScheduleService.cs
│   └── Services/          # Реализации сервисов
│       ├── AnimalService.cs
│       ├── AnimalTransferService.cs
│       ├── EnclosureService.cs
│       ├── FeedingOrganizationService.cs
│       ├── FeedingScheduleService.cs
│       └── ZooStatisticsService.cs
│
├── Domain/
│   ├── Entities/          # Доменные сущности
│   │   ├── Animal.cs
│   │   ├── Enclosure.cs
│   │   ├── Entity.cs      # Базовый класс для сущностей
│   │   └── FeedingSchedule.cs
│   ├── Events/            # Доменные события
│   │   ├── AnimalMovedEvent.cs
│   │   ├── DomainEvent.cs
│   │   ├── DomainEvents.cs
│   │   ├── FeedingTimeEvent.cs
│   │   └── IDomainEventHandler.cs
│   ├── Interfaces/        # Интерфейсы репозиториев
│   │   ├── IAnimalRepository.cs
│   │   ├── IEnclosureRepository.cs
│   │   └── IFeedingScheduleRepository.cs
│   └── ValueObjects/      # Объекты-значения
│       ├── EnclosureType.cs
│       ├── FoodType.cs
│       └── Gender.cs
│
├── Infrastructure/
│   ├── Data/              # Доступ к данным
│   │   └── InMemoryContext.cs
│   └── Repositories/      # Реализации репозиториев
│       ├── AnimalRepository.cs
│       ├── EnclosureRepository.cs
│       └── FeedingScheduleRepository.cs
│
├── Presentation/
│   └── Controllers/       # API контроллеры
│       ├── AnimalController.cs
│       ├── EnclosureController.cs
│       ├── FeedingScheduleController.cs
│       └── ZooStatisticsController.cs
│
└── Tests/                 # Тесты
    ├── Application/
    │   └── Services/
    │       ├── AnimalTransferServiceTests.cs
    │       ├── FeedingOrganizationServiceTests.cs
    │       └── ZooStatisticsServiceTests.cs
    └── Domain/
        ├── Entities/
        │   ├── AnimalTests.cs
        │   ├── EnclosureTests.cs
        │   └── FeedingScheduleTests.cs
        └── ValueObjects/
            └── ValueObjectTests.cs
```

## 💻 Технический стек

### Основные технологии
- **Платформа**: .NET 9.0
- **Язык программирования**: C# 12
- **Архитектурный подход**: Domain-Driven Design (DDD)

### Инструменты и библиотеки
- **Тестирование**: xUnit, Moq
- **Хранение данных**: In-Memory Database (для демонстрации)
- **Внедрение зависимостей**: встроенный DI-контейнер .NET

## 🚀 Установка и запуск

### Предварительные требования
- .NET 9.0 SDK или выше
- Любая IDE с поддержкой C# (рекомендуется Visual Studio 2022, Visual Studio Code с расширением C#)

### Шаги для локальной установки

1. **Клонирование репозитория**
   ```bash
   git clone 
   cd ZooManagementSystem
   ```

2. **Восстановление зависимостей и сборка проекта**
   ```bash
   dotnet restore
   dotnet build
   ```

3. **Запуск приложения**
   ```bash
   dotnet run
   ```

4. **Доступ к API**
   По умолчанию API будет доступен по адресу `http://localhost:5000`

## 🧪 Тестирование

Проект содержит обширный набор модульных тестов, охватывающих основные компоненты системы.

### Запуск всех тестов
```bash
dotnet test
```

### Анализ покрытия кода
```bash
dotnet test --collect:"XPlat Code Coverage"
```

### Примеры тестов

#### Тестирование объектов-значений
```csharp
[Fact]
public void FoodType_Creation_ValidName_CreatesFoodType()
{
    // Arrange & Act
    var foodType = FoodType.Create("Meat");
    
    // Assert
    Assert.Equal("Meat", foodType.Name);
}
```

#### Тестирование бизнес-правил для вольеров
```csharp
[Fact]
public void Enclosure_AddAnimal_WrongEnclosureType_ReturnsFalse()
{
    // Arrange
    var enclosure = new Enclosure(EnclosureType.Herbivore, 100, 5);
    var animal = new Animal("Lion", "Simba", DateTime.Now.AddYears(-5), Gender.Male, FoodType.Create("Meat"));
    
    // Act
    var result = enclosure.AddAnimal(animal);
    
    // Assert
    Assert.False(result);
    Assert.Empty(enclosure.Animals);
}
```

## 📝 Примеры использования API

### Создание нового животного
```http
POST /api/animals
Content-Type: application/json

{
  "species": "Lion",
  "name": "Simba",
  "birthDate": "2020-05-15T00:00:00Z",
  "gender": "Male",
  "favoriteFood": "Meat"
}
```

### Получение списка вольеров
```http
GET /api/enclosures
```

### Создание расписания кормления
```http
POST /api/feeding-schedules
Content-Type: application/json

{
  "animalId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "foodType": "Meat",
  "feedingTime": "14:30:00",
  "scheduleDate": "2025-04-18"
}
```

### Перемещение животного между вольерами
```http
POST /api/animal-transfers
Content-Type: application/json

{
  "animalId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "sourceEnclosureId": "1fa85f64-5717-4562-b3fc-2c963f66afa7",
  "destinationEnclosureId": "2fa85f64-5717-4562-b3fc-2c963f66afa8"
}
```

## 👥 Вклад в проект

Мы рады любым вкладам в развитие ZooManagementSystem! Вот как вы можете помочь:

1. **Форкните репозиторий**
2. **Создайте ветку с новой функциональностью**
   ```bash
   git checkout -b feature/amazing-feature
   ```
3. **Внесите свои изменения**
4. **Закоммитьте изменения**
   ```bash
   git commit -m 'Add some amazing feature'
   ```
5. **Отправьте изменения в свой форк**
   ```bash
   git push origin feature/amazing-feature
   ```
6. **Откройте Pull Request**

### Правила участия
- Пишите чистый, читаемый код
- Следуйте стилю именования и структуре проекта
- Обязательно покрывайте новый код тестами
- Убедитесь, что все тесты проходят успешно

## 📄 Лицензия

Проект ZooManagementSystem распространяется под лицензией MIT. Подробности можно узнать в файле [LICENSE.md](LICENSE.md).

---

© 2025 ZooManagementSystem. Все права защищены.
