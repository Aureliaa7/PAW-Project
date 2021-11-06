function get_enrolled_student_name() {
    var course_title = { id: $("#selected_course").val() };
    var student_cnp = { id: $("#selected_cnp").val() };
    var enrollment_details = {
        courseTitle: $("#selected_course").val(),
        studentCnp: $("#selected_cnp").val(),
    };
    console.log(enrollment_details);
    $.ajax({
        url: "../Enrollments/GetEnrolledStudentName",
        type: "GET",
        data: enrollment_details,
        success: function (data) {
            $("#student_name").val(data);
            console.log(data);
        },
        error: function () {
            console.log("Something went wrong");
        }
    });
}

function get_student_to_be_deleted() {
    var study_year = { id: $("#study_year_field").val() };
    var section = { id: $("#section_field").val() };
    var selected_cnp = { id: $("#cnp_field").val() };
    var student_details = {
        studyYear: $("#study_year_field").val(),
        sectionName: $("#section_field").val(),
        cnp: $("#cnp_field").val(),
    };
    console.log(student_details);
    $.ajax({
        url: "../Students/GetStudentToBeDeleted",
        type: "GET",
        data: student_details,
        success: function (data) {
            $("#student_name_field").val(data);
            console.log(data);
        },
        error: function () {
            console.log("Something went wrong");
        }
    });
}

function get_secretary_name() {
    var cnp_field = { id: $("selected_cnp").val() };
    var secretary_details = {
        cnp: $("#selected_cnp").val(),
    };
    console.log(secretary_details);
    $.ajax({
        url: "../Secretaries/GetSecretaryNameByCnp",
        type: "GET",
        data: secretary_details,
        success: function (data) {
            $("#secretary_name").val(data);
            console.log(data);
        },
        error: function () {
            console.log("Something went wrong");
        }
    });
}


function get_info() {
    var course_title = { id: $("#course_field").val() };
    var teacher_cnp = { id: $("#cnp_field").val() };
    var course_assignment_details = {
        courseTitle: $("#course_field").val(),
        teacherCnp: $("#cnp_field").val(),
    };
    console.log(course_assignment_details);
    $.ajax({
        url: "../TeachedCourses/GetInfo",
        type: "GET",
        data: course_assignment_details,
        success: function (data) {
            $("#name_field").val(data);
            console.log(data);
        },
        error: function () {
            console.log("Something went wrong");
        }
    });
}


function GetUserPicture() {
    $(document).ready(function () {
        $.ajax({
            url: "/Account/GetUserImage",
            type: "GET",
            success: function (data) {
                console.log(data);
                if (document.getElementById("UserPic") != null) {
                    if (data != null) {
                        document.getElementById("UserPic").src = data;
                    }
                }
            },
            error: function () {
                console.log("Something went wrong...");
            }
        });
    });
}
