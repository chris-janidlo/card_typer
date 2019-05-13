#!/usr/bin/env python3

from string import ascii_uppercase
from itertools import chain
from sys import argv
from pyperclip import copy

special_keys = ['Backspace', 'Return', 'Space']

properties = ''
enumerator = '\tpublic IEnumerator<KeyState> GetEnumerator()\n\t{\n'

for key in chain(special_keys, ascii_uppercase):
	state_type = 'Deactivated' if key.upper() in map(str.upper, argv[1:]) else 'Active'
	state_name = f'{key}State'
	properties += f'\t[SerializeField]\n\tprivate KeyState {state_name} = new KeyState {{ Type = KeyStateType.{state_type} }};\n'
	enumerator += f'\t\tyield return {state_name};\n'

copy(f'#region AUTO_GENERATED_FIELDS\n{properties}\n{enumerator}\t}}\n#endregion')
