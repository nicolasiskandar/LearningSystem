Online Learning System following Clean Architecture and DDD. To run:

```bash
dotnet restore
```

```bash
dotnet ef database update --project LearningSystem.Infrastructure --startup-project LearningSystem.Api
```

```bash
docker compose up -d
```

```bash
dotnet run --project LearningSystem.Api
```

```bash
cd LearningSystem.Web && npm start
```
