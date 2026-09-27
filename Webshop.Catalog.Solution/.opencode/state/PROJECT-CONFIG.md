# Project Configuration

Agents read this file at startup to understand project type and technology choices.
Created once during project initialization. Modify via `ucn-consultant` with `/reconfigure-project`.

---

## Project Type

**Track:** rapid

Options:
- `rapid` = minimal governance, fast iteration, optional reviews
- `production` = full governance, required reviews, comprehensive testing

---

## Technology Stack

### Frontend
- **Framework:** none
- **State:** n/a
- **CSS:** n/a
- **Router:** n/a

Options: `nuxt` | `vue` | `none`

### Backend
- **API:** controller-api
- **Framework:** .NET 10
- **ORM:** dapper + entity framework (mixed)

Options: `minimal-api` | `controller-api`

### Database
- **Type:** mssql
- **Connection:** Docker Compose (see docker-compose.yml)

Options: `postgresql` | `sqlite` | `mssql`

---

## Governance Rules

Based on track setting and project maturity. Agents apply these rules automatically.

**Current Governance Level:** *(auto-adjusted based on maturity)*

### Rapid Track Defaults

```yaml
reviews:
  code-review: skip
  architecture-review: ask
  security-review: skip
documentation: no
testing: minimal
coverage: 0%
release-manager: skip
```

### Production Track Defaults

```yaml
reviews:
  code-review: ask
  architecture-review: automatic
  security-review: ask
documentation: yes
testing: standard
coverage: 70%
release-manager: ask
```

---

## Dynamic Governance (Auto-Adjustment)

Governance level adjusts based on project maturity and risk.

**Maturity Indicators:**

| Indicator | Threshold | Effect |
|-----------|-----------|--------|
| Tasks completed | > 50 | Increase test coverage requirement |
| Security issues found | > 3 | Enable automatic security review |
| Architecture changes | > 5 ADRs | Enable automatic architecture review |
| Test failures | > 20% | Increase testing rigor |
| Code review rejections | > 30% | Enable automatic code review |

**Risk-Based Escalation:**

Temporarily increase governance when:
- Working on `[security]` tagged tasks (enable @security even in rapid)
- Working on database migrations (enable @reviewer)
- Working on authentication/authorization (enable @security + @reviewer)
- Deployment to production (enable @release-manager)

**Emergency Mode:**

Temporarily reduce governance when:
- Critical bug fix in production
- Time-critical demo
- Experimental spike

**Activation:** User sets `governance-override: emergency` in this file

---

## Governance Override

**Manual override:** User can adjust governance temporarily

```yaml
governance-override:
  active: false
  mode: emergency | strict
  reason: "Critical production bug - reduce gates"
  expires: YYYY-MM-DD
```

When `active: true`:
- `emergency` mode: Skip all optional gates, minimal testing
- `strict` mode: Enable all gates regardless of track

**Auto-expires:** Returns to normal governance after expiry date

---

## Active Configuration

**Track:** rapid

**Tech Stack:**
- Frontend: none (API-only, Help service has minimal UI)
- Backend API: controller-api (.NET 10 microservices)
- Database: mssql (Docker Compose)

**Governance:**
- Reviews: skip (rapid track)
- Documentation: minimal (brownfield demo project)
- Testing: build + existing tests only

---

## Configuration History

- 2026-09-27: Initial configuration for brownfield microservices demo project
  - Track: rapid (demo/teaching purpose)
  - Backend: controller-api (.NET 10)
  - Database: mssql (Docker)
  - Frontend: none (API-focused)

---

*Created: 2026-09-27*
*Last modified: 2026-09-27*
