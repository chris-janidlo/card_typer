# Cloning

This project uses symlinks, so if you're on Windows, make sure you have symlink support installed (see [here](https://stackoverflow.com/a/42137273/5931898)).

Windows:

	`git clone --recurse-submodules -c core.symlinks=true https://github.com/crassSandwich/card_typer`

*nix:

	`git clone --recurse-submodules https://github.com/crassSandwich/card_typer`

After cloning, run `build.py` with Python 3 to generate some required source code files.
