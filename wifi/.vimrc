let nl_root="$(pwd)"
let nl_target="x86_64-apple-darwin"
let nl_out="out/" . nl_target
let g:cmake_build_dir_location=nl_out
let g:cmake_default_config=""
let input_project="assets/nila_sample"
let document_id=""
let output_project=nl_root . "/out"
let img1=output_project . "/img1.png"
let img2=output_project . "/img2.png"
let document_index=1
let nl_out_type="png"
set makeprg=cmake\ --build\ out\
set errorformat=\ %#%f(%l\\\,%c):\ %m

set path+=**
set wildmenu
set wildignore+=**/out/**,*.obj,*.a

function CopyHelper()
    !cp -rv wasm/js/helper ~/Build/ROOT/swcore
    exec "!cp -rv " . g:nl_out . "/sw-core.* ~/Build/ROOT/swcore/"
endfunction

function CopyHelperJS()
    !cp -rv wasm/js/helper ~/Build/ROOT/swcore
    exec "!cp -rv " . g:nl_out . "/sw-core.* ~/.local/share/gcbuild/test"
endfunction

function ConfigureAndBuild()
    exec "!cmake -DCMAKE_BUILD_TYPE=Release --preset " . g:nl_target
    CMakeBuild
endfunction

function CMakePresetDebug()
    exec "!cmake -DCMAKE_BUILD_TYPE=Debug --preset " . g:nl_target
endfunction

function SaveAndBuild()
    wall
    CMakeBuild
endfunction

function RunExecutable()
    " wincmd s
    " wincmd v
    exec "!./" . g:nl_out . "/nl-nila --log-level 2 --document-id \"" . g:document_id . "\" --input \"" . g:input_project . "\" >>out/nl-nila.log 2>>out/nl-nila.err &"
    "TODO Open log
endfunction

function RunNilaDevToolImage()
    exec "!./" . g:nl_out . "/nl-nila --input " . g:input_project . " --output " g:output_project " -l 2 --type png"
endfunction

function RunNilaDevToolPDF()
    exec "!./" . g:nl_out . "/nl-nila --input " . g:input_project . " --output " g:output_project " -l 2 --type pdf"
endfunction

function RunNilaDevToolSVG()
    exec "!./" . g:nl_out . "/nl-nila --input " . g:input_project . " --output " g:output_project " -l 2 --type svg"
endfunction

function RunUtilityDumpDiffObject()
    exec "!./" . g:nl_out . "/utilities/dumpdiffobject -1 " . g:img1 . " -2 " g:img2 . " -l 2 --type " . g:nl_out_type
endfunction

function BuildAndRun()
    call SaveAndBuild()
    call RunExecutable()
endfunction
