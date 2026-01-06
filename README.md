# Library Management System

RESTful API pro správu knihovny postavené na ASP.NET Core s Entity Framework Core a SQLite databází.

## Popis projektu

Aplikace Library Management System umožňuje komplexní správu knihovny včetně:
- Správa autorů - vytváření, úpravy a mazání autorů
- Správa knih - evidence knih s ISBN, dostupností a skladovými informacemi
- Správa půjček - sledování aktivních půjček, historie a upozornění na prodlení
- Statistiky - přehledy o nejpůjčovanějších knihách a celkových statistikách knihovny

## Technologie a architektura

- Framework: ASP.NET Core (.NET 10)
- Databáze: SQLite s Entity Framework Core
- Architektura: Repository pattern, Service layer, Dependency Injection
- Dokumentace API: Swagger/OpenAPI

## Požadavky

- .NET 10 SDK nebo vyšší
- SQLite (není nutná instalace, je součástí projektu)

## Návod na spuštění

### 1. Klonování repozitáře
cmd:
```
git clone https://github.com/xcntroller/LibraryManagementSystem.git
cd LibraryManagementSystem/LibraryManagementSystem
```

### 2. Obnovení dependencies
Powershell v rootu projektu:
```
dotnet restore
```

### 3. Migrace databáze
Powershell v rootu projektu:
```
dotnet-ef database update
```
pokud není nainstalován dotnet-ef:
```
dotnet tool install --global dotnet-ef
```

### 4. Spuštění aplikace
Powershell v rootu projektu:
```
dotnet run
```

Aplikace se spustí na:
- HTTPS: `https://localhost:7106`
- HTTP: `http://localhost:5091`
- Swagger UI: `https://localhost:5091/swagger`

## API Dokumentace

### Autoři

#### GET `/api/Authors`
Získání seznamu všech autorů s podporou stránkování a filtrování.

*Query parametry:*
- `pageNumber` (optional): Číslo stránky (min: 1)
- `pageSize` (optional): Počet záznamů na stránku (1-100)
- `filter` (optional): Fulltextové vyhledávání

*Příklad požadavku:*
GET https://localhost:5091/api/Authors?pageNumber=1&pageSize=20&filter=George%20Orwell


#### GET `/api/Authors/{id}`
Získání detailu konkrétního autora.

*Příklad požadavku:*
GET https://localhost:5091/api/Authors/1

#### POST `/api/Authors/add`
Vytvoření nového autora.

*Příklad požadavku:*
POST https://localhost:5091/api/Authors/add Content-Type: application/json
`{ "firstName": "J.R.R.", "lastName": "Tolkien", "description": "Anglický spisovatel a profesor", "birthYear": 1892 }`

#### PUT `/api/Authors/{id}`
Aktualizace informací o autorovi.

*Příklad požadavku:*
PUT https://localhost:5091/api/Authors/1 Content-Type: application/json
`{ "firstName": "J.R.R.", "lastName": "Tolkien", "description": "Legendární anglický spisovatel a filolog", "birthYear": 1892 }`

#### DELETE `/api/Authors/{id}`
Smazání autora (pouze pokud nemá přiřazené knihy).

*Příklad požadavku:*
DELETE https://localhost:5091/api/Authors/1

---

### Knihy

#### GET `/api/Books`
Získání seznamu všech knih s podporou stránkování a filtrování.

*Query parametry:*
- `pageNumber` (optional): Číslo stránky
- `pageSize` (optional): Počet záznamů na stránku (1-100)
- `filter` (optional): Fulltextové vyhledávání

*Příklad požadavku:*
GET https://localhost:5091/api/Books?pageNumber=1&pageSize=20&filter=Pán%20prstenů

#### GET `/api/Books/{id}`
Získání detailu konkrétní knihy.

*Příklad požadavku:*
GET https://localhost:5091/api/Books/5


#### GET `/api/Books/isbn/{isbn}`
Získání knihy podle ISBN.

*Příklad požadavku:*
GET https://localhost:5091/api/Books/isbn/9780261102354


#### GET `/api/Books/{id}/available`
Kontrola dostupnosti knihy ke výpůjčce.

*Příklad požadavku:*
GET https://localhost:5091/api/Books/5/available

*Odpověď:*
`{ "bookId": 5, "isAvailable": true }`


#### GET `/api/Books/author/{id}`
Získání všech knih podle ID autora.

*Příklad požadavku:*
GET https://localhost:5091/api/Books/author/1

#### POST `/api/Books/add`
Vytvoření nové knihy.

*Příklad požadavku:*
POST https://localhost:5091/api/Books/add Content-Type: application/json
`{ "name": "Pán prstenů: Společenstvo prstenu", "isbn": "9780261102354", "description": "První díl epické fantasy trilogie", "publicationYear": 1954, "pcsTotal": 10, "authorId": 1 }`

#### PUT `/api/Books/{id}`
Aktualizace informací o knize.

*Příklad požadavku:*
PUT https://localhost:5091/api/Books/5 Content-Type: application/json
`{ "name": "Pán prstenů: Společenstvo prstenu", "isbn": "9780261102354", "description": "První díl epické fantasy trilogie - aktualizováno", "publicationYear": 1954, "pcsTotal": 15, "authorId": 1 }`

#### DELETE `/api/Books/{id}`
Smazání knihy (pouze pokud nemá aktivní výpůjčky).

*Příklad požadavku:*
DELETE https://localhost:5091/api/Books/5

---

### Půjčky

#### GET `/api/Loans`
Získání seznamu všech půjček s podporou stránkování a filtrování.

*Query parametry:*
- `pageNumber` (optional): Číslo stránky
- `pageSize` (optional): Počet záznamů na stránku (1-100)
- `filter` (optional): Filtr podle jména půjčujícího

*Příklad požadavku:*
GET https://localhost:5091/api/Loans?pageNumber=1&pageSize=10

#### GET `/api/Loans/{id}`
Získání detailu konkrétní půjčky.

*Příklad požadavku:*
GET https://localhost:5091/api/Loans/1

#### GET `/api/Loans/active`
Získání seznamu všech aktivních půjček.

*Příklad požadavku:*
GET https://localhost:5091/api/Loans/active


#### GET `/api/Loans/overdue`
Získání seznamu všech půjček po splatnosti.

*Příklad požadavku:*
GET https://localhost:5091/api/Loans/overdue

#### GET `/api/Loans/books/{id}/active`
Získání aktivních půjček konkrétní knihy.

*Příklad požadavku:*
GET https://localhost:5091/api/Loans/books/5/active

#### GET `/api/Loans/member?memberName={name}`
Získání historie půjček konkrétního člena.

*Příklad požadavku:*
GET https://localhost:5091/api/Loans/member?memberName=Jan%20Novák

#### GET `/api/Loans/books/{id}`
Získání historie půjček konkrétní knihy.

*Příklad požadavku:*
GET https://localhost:5091/api/Loans/books/5

#### POST `/api/Loans/add`
Vytvoření nové půjčky (automatická výpůjční doba 14 dní).

*Příklad požadavku:*
POST https://localhost:5091/api/Loans/add Content-Type: application/json
```{ "bookId": 5, "memberName": "Jan Novák" }```

#### PUT `/api/Loans/{id}/return`
Vrácení vypůjčené knihy.

*Příklad požadavku:*
PUT https://localhost:5091/api/Loans/1/return

---

### Statistiky

#### GET `/api/Statistics/most-borrowed`
Získání nejpůjčovanějších knih.

*Query parametry:*
- `topCount` (optional, default: 10): Počet knih (1-100)

*Příklad požadavku:*
GET https://localhost:5091/api/Statistics/most-borrowed?topCount=10

#### GET `/api/Statistics/loans`
Získání celkových statistik výpůjček.

*Příklad požadavku:*
GET https://localhost:5091/api/Statistics/loans

*Odpověď:*
`{ "totalLoans": 150, "activeLoans": 23, "overdueLoans": 5, "returnedLoans": 127 }`

#### GET `/api/Statistics/library`
Získání celkových statistik knihovny.

*Příklad požadavku:*
GET https://localhost:5091/api/Statistics/library

*Odpověď:*
`{ "totalBooks": 500, "totalCopies": 1250, "availableCopies": 780, "totalAuthors": 85 }`

##  Datový model

Aplikace pracuje se třemi hlavními entitami:

### Author (Autor)
- `Id` - Primární klíč
- `FirstName` - Křestní jméno
- `LastName` - Příjmení
- `Description` - Popis autora
- `BirthYear` - Rok narození
- `Books` - Kolekce knih autora

### Book (Kniha)
- `Id` - Primární klíč
- `Name` - Název knihy
- `ISBN` - ISBN kód (validace 10 nebo 13 číslic)
- `Description` - Popis knihy
- `PublicationYear` - Rok vydání
- `PcsTotal` - Celkový počet kusů
- `PcsInStock` - Počet dostupných kusů
- `AuthorId` - Cizí klíč na autora
- `Author` - Reference na autora
- `Loans` - Kolekce výpůjček knihy

### Loan (Půjčka)
- `Id` - Primární klíč
- `LoanDate` - Datum výpůjčky
- `ReturnDate` - Očekávané datum vrácení
- `ReturnedAt` - Skutečné datum vrácení (nullable)
- `BookId` - Cizí klíč na knihu
- `Book` - Reference na knihu
- `MemberName` - Jméno člena (bez autentizace)

### Vztahy mezi entitami
- Author → Book: 1:N (Jeden autor může mít více knih)
- Book → Loan: 1:N (Jedna kniha může mít více výpůjček)

##  Co by se dalo vylepšit
### 1. **Autentizace a základní zabezpečení**
Aktuálně systém nemá žádnou autentizaci. Půjčky jsou spárovány pouze podle jména člena (`MemberName`), což není ideální. Byla to sice část zadání, nicméně by to byla první věc kterou bych co nejdříve implementoval.
- Vytvořil bych entitu 'Uživatel' s unikátním ID a přihlašovacími údaji
- Přidal bych role Admin, Knihovník a Uživatel pro různé úrovně přístupu. Momentálně může kdokoliv přidávat a mazat.

### 2. **Pokročilé vyhledávání a filtry**
Rožšířil bych možnosti vyhledávání:
- Řazení podle popularity
- Systém tagů a kategorií
- Filtry podle žánrů, roku vydání a dostupnosti

### 3. Konfigurační soubor
Přidal bych lepší konfiguraci projektu:
- Místo const proměnných v samotných souborech bych přidal config soubor, aby se všechny proměnné které by uživatel mohl chtít změnit (například LoanPeriod) daly jednoduše změnit

##  Licence

Tento projekt byl vytvořen jako ukázková aplikace.

---

**Autor:** Vojtěch Čermák 
**Repozitář:** [LibraryManagementSystem](https://github.com/xcntroller/LibraryManagementSystem)