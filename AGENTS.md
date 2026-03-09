# AGENTS.md - Development Guidelines

## Project Overview
- **Type:** .NET MAUI application (cross-platform mobile/desktop)
- **Frameworks:** net9.0-android, net9.0-ios, net9.0-maccatalyst, net9.0-windows10.0.19041.0
- **Root Namespace:** `Culinary_Guide`
- **Solution File:** `Culinary Guide.sln`

## Build Commands

### Build Project
```bash
dotnet build "Culinary Guide.sln"
dotnet build "Culinary Guide\Culinary Guide.csproj"
```

### Build for Specific Platform
```bash
dotnet build -f net9.0-android
dotnet build -f net9.0-ios
dotnet build -f net9.0-windows10.0.19041.0
```

### Run Application
```bash
dotnet run --project "Culinary Guide\Culinary Guide.csproj"
```

### Clean Build
```bash
dotnet clean "Culinary Guide.sln"
dotnet restore "Culinary Guide.sln"
```

## Testing
- No test project currently exists in this repository
- To add tests: create xUnit test project referencing the main project
- Run tests: `dotnet test` (when tests are added)
- Run single test: `dotnet test --filter "FullyQualifiedName~TestName"`

## Code Style Guidelines

### C# Conventions

#### Naming
- **Classes/Structs:** PascalCase (e.g., `MainPage`, `MauiProgram`)
- **Methods:** PascalCase (e.g., `CreateMauiApp`, `OnCounterClicked`)
- **Properties:** PascalCase (e.g., `CounterBtn`, `SemanticScreenReader`)
- **Private fields:** camelCase (e.g., `count`, `builder`)
- **Parameters:** camelCase (e.g., `sender`, `activationState`)
- **Namespace:** PascalCase with underscores for multi-word names (`Culinary_Guide`)

#### File Organization
- One class per file (matching filename)
- XAML code-behind files: `<PageName>.xaml.cs`
- Place files in appropriate folders: `Platforms/`, `Resources/`, `Pages/`, `Services/`

#### Imports
- Use implicit usings enabled in project (`<ImplicitUsings>enable</ImplicitUsings>`)
- Add explicit `using` statements only for non-standard namespaces
- Order: System namespaces first, then third-party, then project namespaces

#### Nullable Reference Types
- Enabled: `<Nullable>enable</Nullable>`
- Use `?` for nullable reference types (e.g., `IActivationState?`)
- Avoid null-coalescing when possible; use pattern matching

#### Formatting
- Braces on new lines for namespaces and types
- 4-space indentation (no tabs)
- Max line length: 120 characters (preferred)
- Use expression-bodied members for simple methods/properties

#### Error Handling
- Use exceptions for exceptional cases only
- Prefer returning result types or null for expected failures
- Log errors using `Microsoft.Extensions.Logging` (see `MauiProgram.cs`)

#### XAML Guidelines
- Use `x:Name` for element references in code-behind
- Define reusable styles in `Resources/Styles/Styles.xaml`
- Use `AppThemeBinding` for light/dark theme support
- Keep code-behind minimal; prefer MVVM pattern for complex logic

#### MAUI-Specific Patterns
- Page lifecycle: Override `OnAppearing()`, `OnDisappearing()`
- Use `SemanticScreenReader` for accessibility
- Platform-specific code goes in `Platforms/<Platform>/` folder
- Use `MainThread.BeginInvokeOnMainThread()` for UI thread operations

#### Configuration
- Project configured in `Culinary Guide.csproj`
- App entry point: `MauiProgram.CreateMauiApp()`
- Shell navigation: `AppShell.xaml`
- Styles: `Resources/Styles/`

## Git Workflow
- Feature branches: descriptive names (e.g., `feature/recipe-search`)
- Commits: imperative mood, concise messages
- PRs required for main branch (if configured)
- Verify status before commits: `git status`
- View recent commits: `git log --oneline -5`

## IDE/Editor Recommendations
- Visual Studio 2022 (v17.14+) or JetBrains Rider
- .NET SDK 9.0.302+
- Enable C#12 features support

## Project Structure
```
Culinary Guide/
├── App.xaml(.cs)          # Application definition
├── AppShell.xaml(.cs)     # Shell navigation
├── MainPage.xaml(.cs)     # Main page
├── MauiProgram.cs         # App entry point
├── Platforms/             # Platform-specific code
├── Resources/
│   ├── AppIcon/           # Application icons
│   ├── Fonts/             # Custom fonts
│   ├── Images/            # Image assets
│   ├── Raw/               # Raw assets
│   ├── Splash/            # Splash screen
│   └── Styles/            # XAML styles
└── Properties/            # App properties
```

## Common Tasks

### Add New Page
1. Create `Pages/<PageName>.xaml` with `ContentPage` root
2. Create `Pages/<PageName>.xaml.cs` code-behind with `partial class`
3. Register in `AppShell.xaml` for navigation

### Debugging
- Use `#if DEBUG` for debug-only code (see `MauiProgram.cs`)
- `Microsoft.Extensions.Logging.Debug` enabled in debug builds
- Check `bin/` and `obj/` for build outputs (both gitignored)

## Cursor/Copilot Rules
- No `.cursor/rules/`, `.cursorrules`, or `.github/copilot-instructions.md` files exist
