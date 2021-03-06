# MyGit v1.04 - My Control Version System

## Popis:

ASP.NET Core applikaction for file version control system - http://peg.aspifyhost.cz

Program umí detekovat změny v lokálním adresáři uvedeném na vstupu.
Při prvním spuštění si program obsah daného adresáře analyzuje a při
každém dalším spuštění bude hlásit změny od svého posledního spuštění.

[![MyGit](https://lh3.googleusercontent.com/yp_Nt6guJyUztks1wE9HgoaLnI6lhhXv5ya4CTim79o2nxZUIPqmeBpW2xC9en-J_oPs0tkRnUD6-tSMvMrm1S7o6Pp4_ybttm7R_IKNBwFZqNLDapgMsUnuoISaet4a3uFYUEnPhhM=w2400)](http://peg.aspifyhost.cz)

## Ovládání:

- Check - detekce změn v adresáři včetně podadresářů
- Init - inicializace programu - všechny soubory nastaveny na verzi 1
- Versions - vypis verzí jednotlivých souborů
- Dirs - výpis adresáře včetně podadresářů
- Touch file - aktualizace souboru
- Delete file - smazání souboru
- Set Path - nastaví "Path or File:" na výchozí hodnotu ("\\dir1\\")
- Clear Log - nastaví "Path or File:" na výchozí hodnotu "" a promaže "Log:"

## Rozhraní:

- Path or File: - zadání adresáře nebo souboru pro jednotlivé operace
- Log: - výstupní hlášení programu

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
