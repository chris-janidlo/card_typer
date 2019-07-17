#!/usr/bin/env python3

from sys import argv
from string import ascii_uppercase
from itertools import chain

def generate (output_file):
	special_keys = ['Space', 'Dash', 'Apostrophe', 'Backspace', 'Return']

	properties = ''
	enumerator = ''
	getter = ''

	for key in chain(special_keys, ascii_uppercase):
		state_name = f'{key}State'
		keyboard_key = f'KeyboardKey.{key}'

		properties += f'\tprivate KeyState {state_name} = new KeyState {{ Key = {keyboard_key}, Type = KeyStateType.Active }};\n'
		
		enumerator += f'\t\tyield return {state_name};\n'

		getter += f'\t\t\tcase {keyboard_key}:\n\t\t\t\treturn {state_name};\n'

	# remove extra newlines
	properties = properties[:-1]
	enumerator = enumerator[:-1]
	getter = getter[:-1]

	output = f"""//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace CTShared
{{
public partial class Keyboard : IEnumerable<KeyState>
{{
{properties}

	public IEnumerator<KeyState> GetEnumerator()
	{{
{enumerator}
	}}

	KeyState getState (KeyboardKey key)
	{{
		switch (key)
		{{
{getter}
			default:
				throw new ArgumentException($"unexpected KeyboardKey {{key}}");
		}}
	}}
}}
}}
"""

	file = open(output_file, 'w')
	file.write(output)

if __name__ == "__main__":
	if len(argv) < 2:
		print("usage: python keyboardCodeGenerator.py output_file")
		quit(1)

	generate(argv[1])
