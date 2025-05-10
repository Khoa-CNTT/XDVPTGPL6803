// Move the character
if (characterController == null) characterController = GetComponentInChildren<CharacterController>();
characterController.Move(moveDirection * Time.deltaTime * jumpOffet);