#!/usr/bin/env python3

from os import chdir
from subprocess import call
from shutil import copy

from shared.src.scripts.card_code_generator import generate as generate_card
from shared.src.scripts.keyboard_code_cenerator import generate as generate_keyboard

# TODO: use os.path

print('Generating code')
generate_card('shared/src/core/(auto generated) Card.cs', 'shared/src/data/CardImplementations.cs')
generate_keyboard('shared/src/core/(auto generated) Keyboard.cs')
print('Done\n')

print('Building shared')
chdir('shared')
if call(['dotnet', 'publish', '-c', 'Release']) != 0:
	quit()
