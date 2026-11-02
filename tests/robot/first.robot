*** Keywords ***
Verify Status
    [Arguments]    ${actual}    ${expected}
    Should Be Equal As Integers    ${actual}    ${expected}


*** Test Cases ***
Valid Status Code
    ${status}=    Set Variable    201
    Verify Status    ${status}    201
    Log    Status verified