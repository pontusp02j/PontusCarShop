import React from 'react';

const EmailVerificationFailure = () => {
    return (
        <div className="email-verification-failure">
            <h2>Email Verification Failed</h2>
            <p>We're sorry, but we were unable to verify your email.</p>
            <p>Please ensure you have followed the link from your email correctly. If you continue to experience issues, please request a new verification email or contact our support team.</p>
        </div>
    )
}

export default EmailVerificationFailure;
