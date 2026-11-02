*** Test Cases ***
Valid Status Code
    ${status}=    Set Variable    201
    Should Be Equal As Integers    ${status}    201
    Log    Status verified