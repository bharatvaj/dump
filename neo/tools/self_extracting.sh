#!/bin/sh
archive_url=https://getsh.org/dotfiles.tar.gz
temp_dotfiles_dir=/tmp/dotfiles
test -d "$temp_dotfiles_dir" || mkdir "$temp_dotfiles_dir"
curl "$archive_url" | tar zx - -C "$temp_dotfiles_dir"
. "$temp_dotfiles_dir"/.profile
