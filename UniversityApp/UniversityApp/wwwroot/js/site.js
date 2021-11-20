function get_enrolled_student_name() {
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
        },
        error: function () {
            console.log("Something went wrong");
        }
    });
}

function get_student_to_be_deleted() {
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
        },
        error: function () {
            console.log("Something went wrong");
        }
    });
}

function get_secretary_name() {
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
        },
        error: function () {
            console.log("Something went wrong");
        }
    });
}


function get_info() {
    var course_assignment_details = {
        courseTitle: $("#course_field").val(),
        teacherCnp: $("#cnp_field").val(),
    };
    $.ajax({
        url: "../TeachedCourses/GetInfo",
        type: "GET",
        data: course_assignment_details,
        success: function (data) {
            $("#name_field").val(data);
        },
        error: function () {
            console.log("Something went wrong");
        }
    });
}


function GetUserPicture() {
    $(document).ready(function () {
        $.ajax({
            url: "/Accounts/GetCurrentUserProfileImage",
            type: "GET",
            success: function (data) {
                if (document.getElementById("UserPic") != null) {
                    if (data != null) {
                        document.getElementById("UserPic").src = data;
                    }
                    else {
                        document.getElementById("UserPic").src = "/images/defaultProfileImage.jpg";
                    }
                }
            },
            error: function () {
                console.log("Something went wrong...");
            }
        });
    });
}


function setNotFoundBackgroundImage() {
    var pageBody = document.getElementById("page-body");
    pageBody.style.backgroundImage = "url('/images/404-image.jpg')";
}

function clearSessionStorage() {
    sessionStorage.clear();
}


function get_teachers_by_course_id() {
    var enrollment_details = {
        courseId: $("#selected_course").val(),
    };
    console.log(enrollment_details);
    $.ajax({
        url: "../TeachedCourses/GetTeachersByCourseId",
        type: "GET",
        data: enrollment_details,
        success: function (data) {
            var s = '<option value="-1">Please select</option>';
            for (var i = 0; i < data.length; i++) {
                s += '<option value="' + data[i].id + '">' + data[i].fullName + '</option>';
            }
            $("#teacher").html(s);  
            console.log(data);
        },
        error: function () {
            console.log("Something went wrong");
        }
    });
}