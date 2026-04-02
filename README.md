# InfraStack.Utility.Dependency

輕量級依賴注入工具庫，支援屬性/ readonly 注入。

## 功能特點

- **修飾詞 readonly 注入**：支援 `readonly` 字段的自動注入
- **屬性注入**：支援只讀屬性的自動注入

## 安裝

```bash
dotnet add package InfraStack.Utility.Dependency
```

## 快速開始

### 1. 實現 IRegistration 接口

```csharp
public class MyContainer : IRegistration
{
    public bool IsRegistered(Type type) 
    {
        // 檢查類型是否已註冊
    }
    
    public object? Resolve(Type type) 
    {
        // 解析並返回實例
    }
}
```

### 2. 設置全局容器

```csharp
var container = new MyContainer();
DependencyInjector.SetRegistration(container);
```

### 3. 使用依賴注入

#### readonly 注入

```csharp
public class MyService
{
    private readonly ILogger _logger;
    private readonly IDatabase _database;
    
    public MyService()
    {
        // 注入所有 readonly 字段
        DependencyInjector.Inject(this);
    }
}
```

#### 直接解析

```csharp
var service = DependencyInjector.Resolve<IMyService>();
```

## API 參考

### DependencyInjector (靜態類)

- `SetRegistration(IRegistration)` - 設置容器實現
- `Inject(object)` - 對對象執行字段注入
- `Resolve<T>()` - 解析類型 T 的實例
- `Resolve(Type)` - 解析指定類型的實例
- `IsContainerRegistered` - 檢查容器是否已設置

### IRegistration (接口)

需要實現的容器接口：

```csharp
public interface IRegistration
{
    bool IsRegistered(Type type);
    object? Resolve(Type type);
}
```

### IDependencyInjector (接口)

依賴注入器核心接口：

```csharp
public interface IDependencyInjector
{
    void Inject(object This);
    object? Resolve(Type Type);
}
```

MIT License
