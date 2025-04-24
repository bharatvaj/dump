for i in *; do [ -d "$i"/.git ] && zip -r "$i"/git.zip "$i"/.git && rm -rf "$i"/.git; done
