# .intake — Input til agentsystemet

Denne mappe er din primære indgang til agentsystemet.

Placer filer her når du vil starte nyt arbejde, beskrive en feature,
rapportere en fejl, dele mødenoter eller give agenten kontekst.

Agenten scanner automatisk denne mappe ved opstart og behandler
alt indhold som råt input til intake-workflowet.

---

## Hvad kan du placere her?

| Filtype | Eksempel | Hvad sker der |
|---|---|---|
| Feature-beskrivelse | `feature-login.md` | Startes som nyt intake |
| Mødenoter | `meeting-2026-05-22.md` | Udtrækkes og struktureres |
| Fejlbeskrivelse | `bug-rapport.md` | Registreres som task/risk |
| Krav fra kunde | `krav-fra-kunde.txt` | Bevares og clarificeres |
| Billeder / screenshots | `mockup.png` | Vedlægges intake-artifact |
| Fri tekst / idéer | `ideer.md` | Fanges og struktureres |

Alle filtyper er velkomne. Agenten håndterer ustruktureret input.

---

## Navngivning

Ingen krav til filnavne. Brug hvad der giver mening for dig:

```
feature-kurv-funktionalitet.md
bug-login-fejler-på-mobil.md
meeting-notes-sprint-planning.md
ideer-til-dashboard.md
```

---

## Statussystem

Filer i `.intake/` har tre tilstande styret af undermapper:

```
.intake/
  ├── (rod)          ← Nye filer du har lagt klar — ikke behandlet endnu
  ├── processed/     ← Behandlet af agenten, artifact oprettet i .artifacts/intake/
  └── archived/      ← Afsluttet / ikke relevant længere
```

Agenten flytter filer til `processed/` når intake-artifact er oprettet.
Du kan selv flytte til `archived/` når du er færdig med dem.

---

## Sådan bruger du det

**1. Skriv din input** — ingen særlig formatering nødvendig:

```markdown
# Login-funktion

Brugere skal kunne logge ind med email og password.
Der skal være en "husk mig" funktion.
Glem ikke at håndtere forkert password korrekt.
```

**2. Gem filen** i `.opencode/.intake/`

**3. Start agenten** — den finder filen automatisk og spørger om den skal behandles

---

## Hvad agenten gør med din fil

1. Læser og bevarer dit originale input uændret
2. Identificerer type (feature / bug / idé / mødenoter / krav)
3. Opretter et struktureret intake-artifact i `.artifacts/intake/`
4. Identificerer åbenlyse ambiguities og manglende information
5. Foreslår næste skridt (clarification, architecture, task)
6. Flytter filen til `processed/`

---

## Tips

- Du behøver ikke formatere input perfekt — råt er fint
- Flere filer behandles i rækkefølge
- Du kan tilføje filer mens agenten kører
- Billeder og PDFs kan vedlægges og refereres i din markdown-fil
