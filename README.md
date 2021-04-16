# MyGit v1.02 - My Control Version System

## Popis:

ASP.NET Core applikaction for file version control system.

Program umí detekovat změny v lokálním adresáři uvedeném na vstupu.
Při prvním spuštění si program obsah daného adresáře analyzuje a při
každém dalším spuštění bude hlásit změny od svého posledního spuštění.

## Hlášky:

[A] = added (nový soubor)

[M] = modified (změněný soubor)

[D] = deleted (odstraněný soubor)

## Realizace:

Program je napsán v ASP.NET Core v2.1. Informace o změnách nesmí být v databázi. Proto jsem zvolil ukládání do souboru ".MyGit.Cvs.Csv" formátu .csv, který se nachází v každém adresáři. Pro práci s .csv jsem použil .NET knihovnu CsvHelper (https://joshclose.github.io/CsvHelper)

## Odkazy:

## GitHub URL - https://github.com/erlep/MyGit

## ASP.NET Web - http://peg.aspifyhost.cz

## made by peg - https://GitHub.com/ErleP
