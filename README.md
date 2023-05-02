# ProjectAPI
DOdakowy opis projektu w ProjektAPI_Michal_Salek

Aplikacja .NET Core REST API z uwierzytelnianiem użytkownika i standardowymi operacjami CRUD

To jest przykładowa aplikacja .NET Core REST API z uwierzytelnianiem i autoryzacją użytkowników zaimplementowanymi przy użyciu JSON Web Tokens (JWT). API umożliwia operacje CRUD na zbiorze zasobów, do których mają dostęp tylko autoryzowani użytkownicy.

Wymagania
Przed uruchomieniem tej aplikacji musisz mieć zainstalowane:

.NET Core SDK (wersja 2.1 lub nowsza)
Microsoft SQL Server lub inny kompatybilny silnik bazodanowy
Pierwsze kroki

Sklonuj to repozytorium na swój komputer.

Otwórz projekt w programie Visual Studio

W pliku appsettings.json zaktualizuj ciąg połączenia do swojej bazy danych.

W konsoli Package Manager wykonaj następujące polecenie, aby zastosować migracje bazy danych:

Update-Database
Zbuduj i uruchom aplikację.

Endpoints
Aplikacja zawiera następujące endpointy REST API:

GET /api/product - pobierz wszystkie pordukt 
GET api/product/{id:int} - pobierz pojedynczy produkt o podanym identyfikatorze 
POST /api/product - dodaj nowy produkt 
PUT /api/product - zaaktualizuj produkt 
DELETE /api/product/{id:int} - usuń produkt o podanym identyfikatorze 

POST /api/login - zaloguj użytkownika  
POST /api/register - zarjestruj użytkownika 
Autoryzacja i uwierzytelnianie

Aplikacja wykorzystuje JWT do uwierzytelniania użytkowników i kontrolowania dostępu do zasobów. Aby uzyskać dostęp do chronionych zasobów, należy zalogować się z poprawnymi danymi uwierzytelniającymi, a następnie przesłać token JWT w nagłówku żądania. Aby uzyskać token JWT, należy zalogować się za pomocą metody 
POST /api/login, przesyłając w ciele żądania poprawne dane uwierzytelniające użytkownika.

Konfiguracja
Konfiguracja aplikacji znajduje się w pliku appsettings.json. Można w nim ustawić m.in. ciąg połączenia do bazy danych, klucz tajny dla algorytmu JWT i inne opcje konfiguracyjne.
