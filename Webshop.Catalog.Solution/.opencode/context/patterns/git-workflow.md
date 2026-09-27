---
domain: conventions
capabilities:
  - patterns
keywords:
  - git
  - branch
  - commit
  - pull request
  - merge
  - conventional commits
  - feature branch
priority: high
cost: low
---

# Git Workflow

## Branch Strategy: Feature Branches

| Branch | Purpose | Lifetime |
|---|---|---|
| `main` | Always deployable, protected | Permanent |
| `feature/{milestone-slug}` | One branch per milestone | Merged → deleted |
| `hotfix/{description}` | Emergency production fixes | Merged → deleted |

## Branch Naming

```
feature/m1-customer-crud
feature/m2-order-management
hotfix/fix-login-redirect
```

Format: `{type}/{short-description}` using kebab-case.

## Commit Message Format (Conventional Commits)

```
feat: add customer creation endpoint
fix: correct validation on order amount
refactor: extract base repository class
chore: update NuGet packages
docs: add API documentation for orders
test: add handler unit tests for CreateOrder
```

| Prefix | When |
|---|---|
| `feat:` | New feature or capability |
| `fix:` | Bug fix |
| `refactor:` | Code restructuring without behavior change |
| `chore:` | Maintenance, dependencies, config |
| `docs:` | Documentation only |
| `test:` | Adding or fixing tests |

## PR Convention

- One PR per milestone (or per feature if milestones are large)
- PR title = milestone name or feature description
- PR description = link to `.artifacts/planning/{milestone}.md`
- Require passing CI before merge
- Squash merge feature branches into `main`

## Workflow

```
1. Create feature branch from main
   git checkout -b feature/m1-customer-crud

2. Implement + commit incrementally
   git commit -m "feat: add Customer entity and repository"
   git commit -m "feat: add CreateCustomer command and handler"

3. Push and create PR
   git push -u origin feature/m1-customer-crud

4. CI passes → squash merge into main

5. Delete feature branch
   git branch -d feature/m1-customer-crud
```

## Rules

- Never commit directly to `main`
- Never force-push to `main`
- Squash merge feature branches (clean history)
- Delete feature branches after merge
- Keep commits small and focused — one logical change per commit
- Always write a meaningful commit message (not "fix" or "update")
- Pull `main` before creating a new feature branch

See also: `github-actions.md` for CI pipeline.
