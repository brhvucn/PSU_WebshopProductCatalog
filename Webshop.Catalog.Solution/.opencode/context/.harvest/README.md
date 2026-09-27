# .harvest — Staging area for harvested context

Filer i denne mappe er placeret her af `harvest`-kommandoen.

De er **søgbare** (discover finder dem) men **ikke kuraterede**.

## Formål

Adskiller rå, agent-genereret viden fra det manuelt kuraterede indhold
i de øvrige context-mapper. Giver dig mulighed for at gennemse og
godkende indhold inden det flyttes til den rigtige placering.

## Workflow

```
harvest -Source "..." -Target patterns
    -> lander i .harvest/patterns/
    -> er søgbar med det samme
    -> du gennemser filen
    -> du flytter den manuelt til context/patterns/ når den er klar
```

## Undermapper

```
.harvest/
  architecture/   <- harvested arkitektur-noter
  patterns/       <- harvested kode-mønstre og regler
  packages/       <- harvested package-dokumentation
  examples/       <- harvested kode-eksempler
  frontend/       <- harvested frontend-viden
  techstack/      <- harvested tech stack-noter
```

## Oprydning

Filer der er promoveret til den kuraterede context slettes herfra manuelt.
Filer der ikke er relevante slettes direkte.
