# TRS Web — QA Tester Take-Home Assignment

*Practical Assessment · Intermediate (QA + Automation)*

## 1. Overview

This exercise asks you to apply your Quality Assurance skills to a small, real web application. We are far less interested in a flawless, exhaustive bug list than in how you think about testing, how you structure your work, and how clearly you communicate what you found.

This is an intermediate role, so we do expect a working automation component.

## 2. Before You Start

### Access

Create your own account through the sign-up flow at <https://trs-web.vercel.app>. Use this account to explore and test the application.

### Use of AI tools

You may use AI assistants (e.g. for drafting or automation scaffolding). However, you must fully understand and be able to explain everything you submit.

### Scope boundaries

- Stay within the demo application. Do not perform load testing, denial-of-service, or intrusive security/penetration testing.

## 3. The Application Under Test

**TRS Web** (Tourvest Retail Services) is a small web-based application. The areas in scope for this assessment are:

- User sign-up and login
- User management
- Hobby types
- Charts

Website: <https://trs-web.vercel.app>

*If anything about the application or the scope is unclear, use your judgement, note your assumptions. You may also reach out with questions.*

## 4. What to Deliver

Six items are listed below. Items 4.1–4.5 are expected; 4.6 is optional but valued. A short summary of each follows with a note on what we are looking for.

### 4.1 Test approach note

A brief written note describing how you approached the assessment: what you decided to focus on and why, what you deliberately left out, which techniques you leaned on (e.g. boundary analysis, equivalence partitioning), and how you prioritised.

### 4.2 Decision table

Identify the conditions, the rule combinations, and state the expected action for each.

### 4.3 Issues, bugs & feature observations

A list of the issues, bugs, and feature observations you found while exploring. Separate genuine defects from suggestions/UX opinions and assign each a severity and priority.

### 4.4 Test cases

Written test cases covering the flow of the application. Aim for a sensible mix of positive, negative, and boundary cases, and make it clear which flow each case belongs to.

### 4.5 Automation

Automate at least a few of the test cases you have written. Use whatever tooling you are comfortable with (Playwright, Webdriver.io). Please include:

- The automation code, in a runnable state.

We care about readable, maintainable automation.

### 4.6 Anything else

Any additional analysis, risk notes, test data considerations, accessibility or responsiveness observations.

## 5. Format & Tooling

Use whatever is easiest and best for you to present your work. A free Azure DevOps account, a spreadsheet, or a plain document are all acceptable. What matters is that your work is clear, organised, and easy to follow. If you use a tool that we cannot open (e.g. a private board), please also export a PDF or spreadsheet copy.

## 6. Submission

- **What to send:** your deliverables (Sections 4.1–4.6) plus the automation code/repository.
- **How to send:** please upload your details to <https://github.com/Tourvest-Retail/trc-autoengineer-sekhwarim>
- **Naming:** include your name in the top-level folder or document title, e.g. "TRS QA Assignment — \<Your Name\>".

## 7. Assessment Format

Your submission is followed by a session with the QA Lead and Software Product Manager, structured as:

- A 30-minute Q&A on QA concepts and your approach.
- A 1-hour interview where you present your documentation and walk through your findings, including a short live look at your automation running.

## 8. How We Evaluate

We assess the submission and the presentation together, weighted roughly as below. The descriptors show what a strong submission looks like in each area.

- Quality and clarity of test case design
- Coverage of core flows and edge cases
- Quality of the decision table and issue list
- Documentation clarity and organization
- Communication during the presentation
- Initiative shown in additional testing or automation

*Good luck — we look forward to reviewing your submission.*

## Appendix A — Bug / Observation template

Suggested fields for each entry. You may adapt this to your tool of choice.

| Field | Description |
|---|---|
| **ID** | Unique identifier (e.g. BUG-001). |
| **Title** | Short, specific summary of the problem. |
| **Type** | Defect / usability / suggestion / question. |
| **Severity** | Impact on the system if it occurs (e.g. Critical / Major / Minor / Trivial). |
| **Priority** | How urgently it should be fixed (e.g. High / Medium / Low). |
| **Environment** | Browser, OS, device, and any relevant conditions. |
| **Preconditions** | State the system must be in before the steps. |
| **Steps to reproduce** | Numbered, so anyone can follow them exactly. |
| **Expected result** | What should happen. |
| **Actual result** | What actually happened. |
| **Evidence** | Screenshot, recording, console/network detail, or reference. |
| **Notes** | Frequency, workaround, suspected cause, related items. |

## Appendix B — Test case template

Suggested fields for each test case.

| Field | Description |
|---|---|
| **ID** | Unique identifier (e.g. TC-LOGIN-003). |
| **Title** | What the case verifies. |
| **Flow / area** | e.g. Sign-up, Login, User management, Hobby types, Charts. |
| **Type** | Positive / negative / boundary. |
| **Priority** | High / Medium / Low. |
| **Preconditions** | Required state and any setup. |
| **Test data** | Specific inputs used. |
| **Steps** | Numbered actions to perform. |
| **Expected result** | The outcome that means the test passes. |

## Appendix C — Decision table example (format only)

A neutral illustration of the format. This is a checkout discount rule, unrelated to TRS Web — do not reuse it. Build your table from the application logic you choose.

| Rule | Registered member? | Order ≥ R500? | Apply 10% discount | Free shipping |
|---|---|---|---|---|
| 1 | Yes | Yes | Yes | Yes |
| 2 | Yes | No | Yes | No |
| 3 | No | Yes | No | Yes |
| 4 | No | No | No | No |

*Read each column top-to-bottom as one rule: the condition values combine to produce the actions on the right. A complete table enumerates every meaningful combination of conditions.*
