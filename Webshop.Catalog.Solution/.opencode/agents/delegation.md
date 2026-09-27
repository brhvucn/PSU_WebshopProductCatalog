# Agent Delegation

## Agent Overview

| Agent | Mode | Visible | Owned by | Approval |
|---|---|---|---|---|
| `ucn-consultant` | primary | Tab | User | — |
| `ucn-developer` | primary | Tab | User | — |
| `architect` | subagent | @mention | consultant, developer | automatic |
| `frontend` | subagent | hidden | developer | automatic |
| `reviewer` | subagent | hidden | developer | ask user |
| `tester` | subagent | hidden | developer | automatic |
| `release-manager` | subagent | hidden | developer | ask user |
| `documentation` | subagent | hidden | developer | ask user |
| `security` | subagent | hidden | developer | ask user |
| `onboarding` | subagent | hidden | user (manual) | — |

Hidden agents do not appear in `@` autocomplete. They are invoked via the Task tool.
`automatic` = invoked without asking the user. `ask user` = OpenCode prompts for approval.

---

## Consultant Delegation

| Subagent | Trigger | Permission |
|---|---|---|
| `architect` | Architecture direction needed during discovery or planning | automatic |
| `reviewer` | Planning review requested | ask user |

Consultant does NOT delegate to: tester, release-manager, developer.

---

## Developer Delegation

| Subagent | Trigger | Permission |
|---|---|---|
| `architect` | Architecture question arises during implementation | automatic |
| `frontend` | Task requires Vue 3 / frontend implementation | automatic |
| `reviewer` | Implementation complete, review desired | ask user |
| `tester` | Every task — mandatory gate (see task-loop.md) | automatic |
| `release-manager` | Ready to deploy | ask user |
| `documentation` | Documentation needs writing or updating | ask user |
| `security` | Security review desired before release | ask user |

---

## Delegation Rules

- `automatic` — subagent is invoked without asking the user
- `ask user` — OpenCode prompts the user for approval before invoking
- If the user declines an `ask`, the primary agent continues without the subagent
- No agent may invoke another primary agent via delegation
