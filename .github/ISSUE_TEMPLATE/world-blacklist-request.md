---
name: World Blacklist Request Form
description: >-
  The official form to request your CVR world be added to the Aristois Risky  
  Function Blacklist System.
title: '[WBR]:'
labels:
  - World Blacklist Request
assignees:
  - WTFBlaze
body:
  - type: input
    id: worldId
    attributes:
      label: CVR World ID
      description: >-
        The GUID / World ID that was assigned to your world upon uploading your
        world to ChilloutVR.
      placeholder: 76250134-e2c6-4b50-bb47-513d7870593a
    validations:
      required: true
  - type: input
    id: worldName
    attributes:
      label: World Name
      description: The name of your world at the time of this form being submitted.
      placeholder: Klüb Ice
    validations:
      required: true
  - type: input
    id: authorId
    attributes:
      label: CVR Author ID
      description: The GUID / Author ID that was assigned to your account upon creation.
      placeholder: 3d421937-52d5-c7bb-cca9-fe27230823f4
    validations:
      required: true
  - type: input
    id: authorName
    attributes:
      label: Author Name
      description: Name of the author's account at the time of this form being submitted.
      placeholder: WTFBlaze
    validations:
      required: true
  - type: checkboxes
    id: agreement
    attributes:
      label: Agreement
      options:
        - label: >-
            By selecting this checkbox you agree that you are the sole /
            official author of the submitted ChilloutVR World uploader. You
            agree to knowledge the fact that your world can be removed from the
            blacklist at any moment for any reason determined by Aristois
            Developers.
          required: true
  - type: markdown
    attributes:
      value: >-
        This template was generated with [Issue Forms
        Creator](https://issue-forms-creator.netlify.app)


---


