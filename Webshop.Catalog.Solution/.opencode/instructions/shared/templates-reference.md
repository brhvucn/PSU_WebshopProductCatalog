# Templates Reference

When producing artifacts, use the appropriate template from `.opencode/templates/`.
Templates define structure — agents fill in content from actual project knowledge.

---

## Delivery Artifacts

| Situation | Template | Output location |
|---|---|---|
| Capturing a decision | `decision-template.md` | `.artifacts/decisions/` |
| Identifying a risk | `risk-template.md` | `.artifacts/risks/` |
| Creating a task | `task-template.md` | `.artifacts/tasks/` |
| Performing a review | `review-template.md` | `.artifacts/reviews/` |
| Logging an improvement | `improvement-template.md` | `.artifacts/improvements/` |
| Processing improvements | `improvement-decision-template.md` | `.artifacts/decisions/` |

---

## Planning Artifacts

| Situation | Template | Output location |
|---|---|---|
| Defining a milestone | `planning/milestone-template.md` | `.artifacts/planning/` |
| Creating a roadmap | `planning/roadmap-template.md` | `.artifacts/planning/` |

---

## Discovery Artifacts

| Situation | Template | Output location |
|---|---|---|
| Documenting business value | `discovery/business-value-template.md` | `.artifacts/discovery/` |
| Identifying stakeholders | `discovery/stakeholder-template.md` | `.artifacts/discovery/` |
| Defining the problem | `discovery/problem-statement-template.md` | `.artifacts/discovery/` |
| Capturing raw intake | `intake/intake-raw-template.md` | `.artifacts/intake/` |

---

## Architecture Artifacts

| Situation | Template | Output location |
|---|---|---|
| Documenting architecture | `architecture/architecture-template.md` | `.artifacts/architecture/` |
| Documenting domain model | `architecture/domain-model-template.md` | `.artifacts/architecture/` |
| Documenting integrations | `architecture/integration-template.md` | `.artifacts/architecture/` |
| C4 context diagram | `architecture/c4-context-template.md` | `.artifacts/architecture/` |

---

## Documentation (human-readable, written to project)

| Situation | Template | Output location |
|---|---|---|
| Project README | `documentation/readme-template.md` | `README.md` |
| Architecture overview | `documentation/architecture-overview-template.md` | `docs/architecture.md` |
| API documentation | `documentation/api-docs-template.md` | `docs/api.md` |
| Architecture Decision Record | `documentation/adr-template.md` | `docs/decisions/ADR-NNN.md` |
| Onboarding guide | `documentation/onboarding-guide-template.md` | `docs/onboarding.md` |

---

## Onboarding Analysis (used by @onboarding agent)

| Situation | Template | Output location |
|---|---|---|
| Project overview | `onboarding/project-overview-template.md` | `.artifacts/discovery/` |
| Feature inventory | `onboarding/feature-inventory-template.md` | `.artifacts/discovery/` |
| Gap analysis | `onboarding/gap-analysis-template.md` | `.artifacts/discovery/` |
| Package inventory | `onboarding/package-inventory-template.md` | `.artifacts/discovery/` |
