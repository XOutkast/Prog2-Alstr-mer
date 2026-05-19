# Projekt01 - Slutprojekt Projektplan

**SidTheSnakeA - Snake-spel i MonoGame**

---

## Betygsambition

Jag siktar på **A-B**.

Jag vill visa att jag kan bygga ett spel i MonoGame med OOP på ett tydligt sätt. Spelet har spel-loop, nivå progression, HighScore i JSON och flera collectibles och hazards. Jag har också försökt göra spelet rättvist så man inte dör direkt av en liten feltryckning.

---

## Projektplan

### Spelidé

Jag tänkte göra ett Snake-spel i MonoGame där man styr en orm, samlar poäng och undviker hinder. Spelet ska vara enkelt i början men bli svårare steg för steg.

### Genomförande

Jag har delat upp spelet i flera klasser så det blir lättare att hålla koll på. Game1 sköter spel-loop, rendering, nivåer och states. Andra klasser sköter collectibles, hazards, highscore och ormen.

Viktiga delar:

- Snake och rörelse: riktningar och direction-queue.
- Collectibles: olika items ger poäng, growth och ibland speed boost.
- Hazards: både fasta och rörliga hinder.
- Nivåprogression: fler väggblock och fler hazards på varje nivå.
- HighScore: sparas i JSON.

### Problem som behövde lösas

- Collision mellan snake, walls, hazards och collectibles.
- Hur input ska buffras utan att det känns konstigt.
- Hur svårigheten ska öka steg för steg.
- Hur HighScore ska sparas och laddas.
- Hur HUD och boost ska synas tydligt.

### Lösningstankar

För att lösa detta har jag använt Point-baserad collision, direction-queue för input, nivålogik som bygger vidare på tidigare nivåer och JSON för highscore. Jag har också försökt hålla koden enkel så den går att felsöka.

### Speluppbyggnad

Spelet använder en 24 x 20 spelplan med celler. Man ser en HUD med poäng och nivå, och spelet uppdateras i en fast loop. När man går upp i nivå läggs lite mer saker till, men inte för mycket.

---

## Poäng

I tabellen nedan har jag skrivit det jag tycker att projektet klarar.

| Kriterium                      | Poäng | Självskattning                           | Bedömning |
| ------------------------------ | ----: | ---------------------------------------- | --------- |
| Highscore                      |    15 | JSON-baserad topplista finns             |           |
| OOP (Arv + Samling)            |    30 | Basklasser och List<T> används           |           |
| Polymorfism                    |    15 | Collectibles och hazards beter sig olika |           |
| Inkapsling                     |    10 | Privata fält och publika metoder         |           |
| Projektkvalitet & Felhantering |    30 | Spelet är stabilt och har bra struktur   |           |

### Självskattning

Jag tycker att projektet uppfyller kraven ganska bra eftersom det har tydlig OOP, fungerande spelmekanik, HighScore, bra uppdelning av ansvar och dokumentation som hänger ihop med spelet.

---

## Dokumentation

### Loggbok

| Datum      | Arbetad tid | Vad har gjorts                      | Hur gick det                        | Vad behöver göras härnäst |
| ---------- | ----------: | ----------------------------------- | ----------------------------------- | ------------------------- |
| 2024-11-13 |          3h | Projektplanering och MonoGame-setup | Bra start                           | Bygga grund-loopen        |
| 2024-11-14 |          4h | Snake-rörelse och kroppssegment     | Rörelsen fungerade bra              | Lägg till collision       |
| 2024-11-15 |          3h | Meny, states och UI                 | Allt gick att växla mellan          | Lägg till collectibles    |
| 2024-11-16 |          4h | Collectibles och polymorfism        | Flera itemtyper fungerade bra       | Lägg till hazards         |
| 2024-11-17 |          3h | Hazard-klasser och collision        | Hinder fungerade som tänkt          | Koppla highscore          |
| 2024-11-18 |          3h | HighScore och JSON-lagring          | Sparning och inläsning funkade      | Förbättra progression     |
| 2024-11-19 |          2h | Nivåprogression och svårighetskurva | Fungerade men behövde balans        | Polera balans och HUD     |
| 2024-11-20 |          3h | HUD med poäng, nivå och boost       | Visningen blev tydlig               | Testning och justering    |
| 2024-11-21 |          2h | Buggfixar och testning              | Spelet blev stabilt                 | Slutlig polering          |
| 2024-11-22 |          2h | Dokumentation och slutkontroll      | Bygget gick igenom och spelet körde | Klart för inlämning       |

---

## UML-diagram

Här ska en bild på UML-diagrammet infogas.

Man kan tänka diagrammet ungefär så här:

- **Game1** sköter spel-loop, nivåer, rendering och state.
- **Snake** sköter ormens rörelse och kropp.
- **Collectible** är basklass för items.
- **Hazard** är basklass för hinder.
- **HighScoreService** sköter sparning och läsning av highscore.

Relationerna är i stort sett så här:

- `Game1` använder `Snake`.
- `Game1` håller listor med `Collectible` och `Hazard`.
- `Game1` använder `HighScoreService`.
- `Collectible` och `Hazard` har egna underklasser.

---

## Sammanfattning

Projektet **SidTheSnakeA** är ett komplett Snake-spel med OOP, stabil spelmekanik och en svårighetskurva som byggs upp steg för steg. Det har highscore, collectibles, hazards och dokumentation.

Jag tycker att det passar bra för min betygsambition eftersom det är genomarbetat och visar att jag kan bygga ett mindre spel själv.

_Dokumentation slutförd: 2026-05-17_
