import React from 'react';

const EmailVerificationSuccess = () => {
    return (
        <div className="email-verification-success">
            <h2>Email Verification Successful</h2>
            <p>Congratulations! Your email verification has been successfully completed.</p>
            <p>Thank you for taking the time to verify your email. This helps us ensure that we can reliably communicate with you, and provide you with a secure experience.</p>
            <a href='/' style={{display: 'inline-block', padding: '10px 20px', color: 'white', backgroundColor: '#1b6ec2', textDecoration: 'none', borderRadius: '5px'}}>
              Continue
              </a>

        </div>
    )
}

export default EmailVerificationSuccess;
