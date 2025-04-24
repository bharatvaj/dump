for i in *; do [ -d "$i"/.git ] && git rm --cached "$i"; done
