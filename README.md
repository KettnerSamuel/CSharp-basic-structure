# ASP.NET Core Maturita Starter Project

Tato struktura projektu slouží jako základ pro pracování v ASP.NET Core. Projekt obsahuje již připravenou základní funkcionalitu pro autentizaci uživatelů, správu účtů a základní strukturu MVC. vše je dělané i s chybovými hláškami.

## Požadavky pro spuštění

Pro správné fungování projektu jsou zapotřebí následující balíčky NuGet (pokud se nenainstalují automaticky):

- BCrypt.Net-Core (v1.6.0) - knihovna pro hashování hesel
- Microsoft.AspNetCore.Session (v2.3.0) - middleware pro správu session v ASP.NET Core
- Microsoft.EntityFrameworkCore (v9.0.3) - ORM framework pro práci s databází
- Microsoft.EntityFrameworkCore.Proxies (v9.0.3) - podpora pro lazy loading v Entity Framework
- Microsoft.EntityFrameworkCore.SqlServer (v9.0.3) - poskytovatel pro MS SQL Server
- Microsoft.EntityFrameworkCore.Tools (v9.0.3) - nástroje pro práci s EF Core v Package Manager Console
- Newtonsoft.Json (v13.0.3) - knihovna pro práci s JSON

## První spuštění

Pro inicializaci databáze postupujte následovně:

1. Otevřete projekt ve Visual Studiu
2. Otevřete Package Manager Console (Tools → NuGet Package Manager → Package Manager Console)
3. Zadejte příkaz: `Update-Database`

Tento příkaz vytvoří databázi podle definovaných migrací a aplikuje základní strukturu.

## Workflow: Přidání nové funkce (příklad: Produkty)

1. **Vytvoř model** (tabulku databáze):
   ```csharp
   // Models/Product.cs
   public class Product
   {
       public int Id { get; set; }
       [Required]
       public string Name { get; set; }
       public string Description { get; set; }
       [Range(0, 100000)]
       public decimal Price { get; set; }
   }
   ```

2. **Zaregistruj v databázi**:
   ```csharp
   // V BasicStructureDbContext.cs přidej:
   public DbSet<Product> Products { get; set; }
   ```

3. **Vytvoř migraci** (v Package Manager Console):
   ```csharp
   Add-Migration AddProducts
   Update-Database

4. **Vytvoř controler (obsluha stránek):
   csharpCopy// Controllers/ProductController.cs
   public class ProductController : Controller
   {
       private readonly BasicStructureDbContext _context;
       
       public ProductController(BasicStructureDbContext context)
       {
           _context = context;
       }
       
       public IActionResult Index()
       {
           var products = _context.Products.ToList(); // Načti produkty
           return View(products); // Předej je do pohledu
       }
       
       public IActionResult Detail(int id)
       {
           var product = _context.Products.Find(id);
           return View(product);
       }
   }

5. Vytvoř pohledy (vzhled stránek):
   htmlCopy<!-- Views/Product/Index.cshtml -->
   @model List<Product>
   
## Shrnutí
   
   Controllers = co se stane při návštěvě URL (akce)
   Models = data a databázové tabulky
   Views = jak stránky vypadají (HTML)
   Data = propojení s databází
   
   Když přidáváš novou funkci, vytvoř:
   
   Model (data)
   Kontroler (akce i pro zpracování dat z databáze)
   Pohledy (vzhled)
   Migraci (aktualizace databáze)

## struktura 
### Controllers
- **HomeController.cs** - kontroler pro hlavní stránky aplikace
- **UserController.cs** - kontroler pro práci s uživatelskými účty
- ostatní si vytvořte dle toho, jak budete potřebovat

### Data
- **BasicStructureDbContext.cs** - kontext pro Entity Framework Core (vlastně není potřeba měnit)

### Models
- **User.cs** - základní model uživatele
- pokud chcete tvořit novou entitu 

### Views
- **Home/** - pohledy pro hlavní stránky
  - **Index.cshtml** - hlavní stránka
  - **Privacy.cshtml** - stránka s informacemi o ochraně osobních údajů
- **Shared/** - sdílené pohledy
  - **_Layout.cshtml** - základní layout pro všechny stránky
  - **_ValidationScriptsPartial** - skripty pro validaci formulářů
  - **Error.cshtml** - stránka pro zobrazení chyb
- **User/** - pohledy pro správu uživatelů
  - **login.cshtml** - přihlašovací formulář
  - **Profile.cshtml** - stránka profilu uživatele
  - **register.cshtml** - registrační formulář

### Další soubory
- **_ViewImports.cshtml** - import direktiv pro Razor
- **_ViewStart.cshtml** - společné nastavení pohledů
- **appsettings.json** - konfigurační soubor aplikace
- **Program.cs** - vstupní bod aplikace

## Implementovaná funkcionalita

Projekt již obsahuje následující implementované funkce:

- **Registrace uživatelů** - vytvoření nového účtu
- **Přihlášení** - autentizace uživatelů
- **Správa účtu** - úprava uživatelského profilu
- **Základní routování** - nastavení cest pro přesměrování

## Před odevzdáním projektu

Před finalizací projektu nezapomeňte:

1. Přepsat obsah stránky Privacy Policy (Views/Home/Privacy.cshtml)
2. přepsat v layoutu dole v patičce autora
3. pokud chcete, můžete si pojmenovat databázi a věci jinak, ale jestli si to rozbijete, je to na vás.
4. Přepsat tento README soubor dle specifik vašeho projektu a jméno do LICENSE.md
   
