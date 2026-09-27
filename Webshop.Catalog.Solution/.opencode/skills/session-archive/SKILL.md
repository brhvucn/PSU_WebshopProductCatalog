---
name: session-archive
description: Archive SESSION.md to .artifacts/sessions/ before overwriting.
---

# Session Archive Skill

```powershell
$id = "s001"  # increment from last archive
Copy-Item -LiteralPath ".opencode/state/SESSION.md" -Destination ".artifacts/sessions/$(Get-Date -Format 'yyyy-MM-dd')-$id.md"
```

## Rules

- Archive BEFORE making changes to SESSION.md
- ID format: s001, s002, ... — read `.artifacts/sessions/` to find next
- Append archive entry to SESSION.md ## Session History table
