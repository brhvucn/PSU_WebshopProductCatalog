# Instruction: Context File Decomposition & Metadata

## Purpose

The purpose of this instruction is to:

* improve semantic retrieval quality
* reduce token overhead
* isolate capability domains
* support bounded context loading
* improve discover/search accuracy

This instruction defines how large context files should be decomposed and enriched with lightweight metadata.

---

# Core Principles

## Single Responsibility

Each context file MUST represent:

* one capability
* one architectural concern
* one technical responsibility
* one conceptual area

Avoid multi-purpose documents.

---

# File Size Guidance

## Preferred Size

Target:

```txt
300–800 tokens
```

per file.

---

## Large Files

Files larger than:

```txt
2000+ tokens
```

SHOULD be evaluated for decomposition.

Files larger than:

```txt
5000+ tokens
```

SHOULD normally be split.

---

# Example

## BAD

```txt
frontend.md
```

Contains:

* routing
* auth
* API service
* Pinia
* Bulma
* forms
* toastr
* conventions

---

## GOOD

```txt
frontend/
  overview.md
  routing.md
  authentication.md
  api-service.md
  state-management-pinia.md
  bulma-layout.md
  toastr-feedback.md
  conventions.md
```

---

# Metadata Section

Each context file SHOULD begin with lightweight metadata.

Example:

```yaml
---
domain: backend

capabilities:
  - cqrs
  - query
  - dapper

keywords:
  - query handler
  - postgres
  - sql

priority: high
cost: low
---
```

---

# Allowed Metadata Fields

Use ONLY lightweight operational metadata.

Recommended fields:

| Field        | Purpose              |
| ------------ | -------------------- |
| domain       | capability isolation |
| capabilities | retrieval scoring    |
| keywords     | semantic search      |
| priority     | ranking              |
| cost         | token awareness      |
| updated      | staleness detection (YYYY-MM-DD, optional) |

Avoid excessive metadata complexity.

---

# Forbidden Metadata Inflation

Do NOT add:

* ontology systems
* long descriptions
* nested semantic structures
* excessive tagging
* AI-generated taxonomy trees

Metadata must remain operational and lightweight.

---

# Retrieval Integration

Search and discovery SHOULD use metadata as part of scoring.

Suggested scoring priority:

1. capability match
2. keyword match
3. filename match
4. priority weighting
5. token cost penalty

---

# Capability Isolation

Frontend, backend, database, architecture, workflows, and packages SHOULD behave as isolated capability domains.

Agents SHOULD retrieve only relevant domains.

---

# Overview Files

When splitting large files, create a small:

```txt
overview.md
```

that explains:

* how the capability area is structured
* major concepts
* navigation guidance

Overview files SHOULD remain concise.

---

# Principle

Context should be:

```txt
small
focused
searchable
composable
```

not monolithic.