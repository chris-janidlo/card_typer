#!/usr/bin/env python3

from string import ascii_uppercase
from itertools import chain
from sys import argv
from pyperclip import copy

special_keys = ['Backspace', 'Return', 'Space']

out = ''

for key in chain(special_keys, ascii_uppercase):
	state_type = 'Deactivated' if key.upper() in map(str.upper, argv[1:]) else 'Active'
	out += f'\tpublic KeyState {key}State = new KeyState {{ Type = KeyStateType.{state_type} }};\n'

copy(out)
