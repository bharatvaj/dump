#!/bin/bash

clang++ gui_test.cpp -o gui_test `fltk-config --ldflags --cxxflags`
