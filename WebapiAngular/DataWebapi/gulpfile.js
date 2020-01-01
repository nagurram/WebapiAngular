/*
This file is the main entry point for defining Gulp tasks and using Gulp plugins.
Click here to learn more. https://go.microsoft.com/fwlink/?LinkId=518007
*/

var gulp = require("gulp");
var ts = require("gulp-typescript");
var tsproject = ts.createProject("tsconfig.json");
var sourcemaps = require("gulp-sourcemaps");

gulp.task("default", function () {
    return tsproject.src()
        .pipe(sourcemaps.init())
        .pipe(tsproject()).js
        .pipe(sourcemaps.write("."))
        .pipe(gulp.dest("app"));
});

gulp.task('dev', function () {
    gulp.watch('app/**/*.ts', ['default']);
});