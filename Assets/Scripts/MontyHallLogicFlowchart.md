# Monti Hall Logic Flowchart
```mermaid
flowchart TD;
    StartGame --> InitializePrice
    InitializePrice --> ChoiceUi
    ChoiceUi --> FirstChoice{FirstChoice?}
    FirstChoice -->|Yes| RevealUi
    FirstChoice -->|No| SwitchUi
    RevealUi --> SwitchUi
    SwitchUi -->|Yes| ChoiceUi
    SwitchUi -->|No| EvalUi
    EvalUi -->|Repeat| StartGame
```