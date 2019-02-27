module.exports = function (grunt) {
    // we want to run the shell command
    // that means command through command prompt
    grunt.loadNpmTasks('grunt-shell');
    grunt.initConfig({
        // read parameters through config file
        // like build path , deploy url
        pkg: grunt.file.readJSON('appsettings.json'),
        shell: {
            command: ["cd <%= pkg.ngbuildpath %>" ,"ng build --watch"].join('&&')
        }
        //"cd <%= pkg.ngbuildpath %>"
        // the above command goes in to the directory
        // of app src folder this is
        // specified in the appsettings.json

        //"ng build --watch"
        // once we are inside that folder we can fire
        // ng build command
    });
    grunt.registerTask('default', ['shell']);
};  
