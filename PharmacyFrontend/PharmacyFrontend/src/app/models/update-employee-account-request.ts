export class UpdateEmployeeAccountRequest {
  currentPassword: string = '';
  newUserName : string = '';
  newPassword: string = '';
  confirmNewPassword: string = '';
  accountId: number = 0;
}