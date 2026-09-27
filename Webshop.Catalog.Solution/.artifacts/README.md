# .artifacts

Durable delivery artifacts produced by the agent system.
Agents write here. Humans review here. Sessions resume from here.

## Structure

| Folder | Content | Written by |
|---|---|---|
| `architecture/` | Architecture documents, ADRs, domain models | architect |
| `decisions/` | Significant decisions with rationale | all agents |
| `discovery/` | Business value, stakeholders, problem statements | consultant |
| `improvements/` | Agent system friction and fix candidates | all agents |
| `intake/` | Raw intake artifacts, preserved original input | consultant |
| `planning/` | Roadmaps, milestones, task lists | consultant |
| `releases/` | Release notes, deployment records | release-manager |
| `reviews/` | Implementation and release review verdicts | reviewer |
| `tasks/` | Individual task progress and status | developer |
| `testing/` | Test reports, coverage analysis, test artifacts | tester, developer |

## Rules

- Artifacts are never deleted — they are the project's history
- Agents read artifacts at session start to resume work
- SESSION.md in `.opencode/state/` points to the active milestone
- When an artifact is superseded, keep the old one and create a new dated file

## Documentation

The `@documentation` subagent reads these artifact folders and synthesises them
into human-readable documentation written directly to the project:

| Output | Location |
|---|---|
| Project README | `README.md` (project root) |
| Architecture overview | `docs/architecture.md` |
| API documentation | `docs/api.md` |
| Architecture Decision Records | `docs/decisions/ADR-NNN-title.md` |
| Onboarding guide | `docs/onboarding.md` |

Artifacts are the **raw material**. `docs/` and `README.md` are the **result**.
