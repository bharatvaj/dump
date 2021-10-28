# Prints out when a file was created using exiftool
when ()
{
    for i in *;
    do
        echo -n "$i";
        exiftool "$i" | grep "Create Date" | head -n 1
		echo ""
    done
}

# Usage
# when | sed '/^$/d'
