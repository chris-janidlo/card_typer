#!/usr/bin/env python3

from string import ascii_uppercase
from itertools import chain
from sys import argv
from pyperclip import copy

special_keys = ['Backspace', 'Return', 'Space']

out = '#region AUTO_GENERATED_FIELDS\n'

for key in chain(special_keys, ascii_uppercase):
	state_type = 'Deactivated' if key.upper() in map(str.upper, argv[1:]) else 'Active'
	out += f'\t[SerializeField]\n\tprivate KeyState {key}State = new KeyState {{ Type = KeyStateType.{state_type} }};\n'

out += '#endregion'

copy(out)
