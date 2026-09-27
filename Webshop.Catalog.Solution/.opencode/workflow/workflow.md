# Workflow

Three fixed phases. Three optional phases activated on demand.

## Phases

| Phase | Agent | Steps | Optional |
|---|---|---|---|
| **1. Understand** | `ucn-consultant` | Intake, Clarification, Discovery, Planning | No |
| **2. Design** | `architect` (subagent) | Architecture | No — invoked by consultant |
| **3. Build** | `ucn-developer` | Implementation, inline tests | No |
| **4. Test** | `tester` (subagent) | Test validation, coverage report | **Yes** — use when dedicated test run is desired |
| **5. Review** | `reviewer` (subagent) | Implementation or release review | **Yes** — use when explicit sign-off is desired |
| **6. Release** | `release-manager` (subagent) | Docker deployment, release notes | **Yes** — use when deployment coordination is desired |

For simple milestones: Build → done. Tests are written inline by the developer.
For complex milestones: Build → Test → Review → Release as needed.

---

## Typical Flows

**Simple feature (most common):**
```
Understand -> Design -> Build
```

**Feature with deployment:**
```
Understand -> Design -> Build -> Release
```

**High-risk or team feature:**
```
Understand -> Design -> Build -> Test -> Review -> Release
```

---

## Agent Flow

```
[User] -> ucn-consultant
              |
              +--> @architect    (automatic — when architecture is needed)
              |
              +--> [Tab] ucn-developer  (when planning is complete)
                        |
                        +--> @architect       (automatic — architecture questions)
                        +--> @frontend        (automatic — when task needs Vue 3)
                        +--> @tester          (optional — ask user)
                        +--> @reviewer        (optional — ask user)
                        +--> @release-manager (optional — ask user)
                        +--> @documentation   (optional — ask user)
```

---

## Switching Agents

Use **Tab** to switch between `ucn-consultant` and `ucn-developer` at any time.

Use `@architect`, `@tester`, `@reviewer`, `@release-manager` to invoke subagents manually.

---

## Release

Most projects in this stack deploy via **Docker / docker-compose**.
See `workflow/release/release.md` for standard deployment steps.

## CI/CD

For automated build/test/deploy pipelines, see `context/patterns/github-actions.md`.
CI runs on every push and PR to `main`. Docker build and deploy are optional stages.

---

## Adopting an Existing Project

If you are starting with an **existing codebase** rather than a new project,
run `@onboarding` before anything else:

```
@onboarding
```

The onboarding agent will:
1. Analyse the existing solution structure, domain, features and packages
2. Generate artifacts in `.artifacts/discovery/` and `.artifacts/architecture/`
3. Produce a gap analysis and suggested first milestones
4. Set SESSION.md to `phase: build` with the first milestone ready

After onboarding completes, switch directly to `ucn-developer` (Tab).
Skip the Understand and Design phases — they are covered by the onboarding artifacts.

**SESSION.md after onboarding:**
```
phase:     build
milestone: [first gap milestone from gap-analysis]
next:      Switch to ucn-developer and begin implementation
```

---

## Principles

- Phases are iterative — return to any earlier phase when needed
- Test, review and release are opt-in — skip them when not needed
- Each phase produces durable artifacts in `.artifacts/`
- SESSION.md is always current — any session can resume from it
