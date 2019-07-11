#!/usr/bin/env python3

# generates states for Keyboard class. creates (or overwrites) file named Keyboard AutoGenerated.cs. Keyboard must remain a partial class

from string import ascii_uppercase
from itertools import chain
from sys import argv

special_keys = ['Space', 'Dash', 'Apostrophe', 'Backspace', 'Return']

header = r"""//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

"""

properties = ''
enumerator = '\tpublic IEnumerator<KeyState> GetEnumerator()\n\t{\n'
getter = '\tKeyState getState (KeyboardKey key)\n\t{\n\t\tswitch (key)\n\t\t{\n'

for key in chain(special_keys, ascii_uppercase):
	state_name = f'{key}State'
	keyboard_key = f'KeyboardKey.{key}'

	properties += f'\tprivate KeyState {state_name} = new KeyState {{ Key = {keyboard_key}, Type = KeyStateType.Active }};\n'
	
	enumerator += f'\t\tyield return {state_name};\n'

	getter += f'\t\t\tcase {keyboard_key}:\n\t\t\t\treturn {state_name};\n'

getter += '\t\t\tdefault:\n\t\t\t\tthrow new ArgumentException($"unexpected KeyboardKey {key}");\n\t\t}\n\t}'

output = header + f'using System;\nusing System.Collections.Generic;\n\nnamespace CTShared\n{{\npublic partial class Keyboard : IEnumerable<KeyState>\n{{\n{properties}\n{enumerator}\t}}\n\n{getter}\n}}\n}}\n'

file = open('Keyboard (auto generated).cs', 'w')
file.write(output)
