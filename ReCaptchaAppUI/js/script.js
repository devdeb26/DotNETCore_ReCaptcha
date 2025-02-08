function LoginButton() {
  const reCaptchaResponse = grecaptcha.getResponse();
  if (reCaptchaResponse) {
    $.ajax({
      type: "GET",
      headers: {
        "Content-Type": "text/plain",
      },
      url: "https://localhost:7130/api/ReCaptchaAPI/Captcha",
      data: { userResponse: reCaptchaResponse },
      success: function (response) {
        console.log(response);
        alert("Captcha Verified");
      },
      error: function (xhr, status, error) {
        console.error("CORS error:", error);
      },
    });
  } else {
    alert("Something went wrong with reCaptcha. Please try again!");
  }
}
