let editor;

window.onload = function () {
    editor = ace.edit("editor");
    editor.setTheme("ace/theme/monokai");
    editor.session.setMode("ace/mode/python");
}

function executeCode() {

    var code = editor.getSession().getValue();

    return code;

}