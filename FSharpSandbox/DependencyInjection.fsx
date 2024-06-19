// Code from this article: https://medium.com/@lanayx/dependency-injection-in-f-the-missing-manual-d376e9cafd0f

open System.Threading.Tasks

// Business types
type UserSettings = {
    NotificationType: NotificationType
}
and NotificationType = 
    | Email of string
    | Sms of string

// Business dependencies
type IGetUserSettings = 
    abstract member GetUserSettings: string -> Task<UserSettings>
type ISendEmail =
    abstract member SendEmail: string * string -> Task<unit>
type ISendSms =
    abstract member SendSms: string * string -> Task<unit>
type IGetAllUserIdsToNotify =
    abstract member GetAllUserIdsToNotify: unit -> Task<string[]>

// Business logic
let getUserSettings (env: IGetUserSettings) userId = 
    env.GetUserSettings(userId)
let sendEmail (env: ISendEmail) address msg = 
    env.SendEmail(address, msg)
let sendSms (env: ISendSms) phone msg = 
    env.SendSms(phone, msg)
let getAllUserIdsToNotify (env: IGetAllUserIdsToNotify) = 
    env.GetAllUserIdsToNotify()

let notifyUser env userId message =
    // env can be defined as (env: #IGetUserSettings & #ISendEmail & #ISendSms) 
    task {
        let! userSettings = getUserSettings env userId
        match userSettings.NotificationType with
        | Email address -> 
            return! sendEmail env address message
        | Sms phone ->
            return! sendSms env phone message
    }

let notifyAllUsers env message =
    task {
        let! userIds = getAllUserIdsToNotify env
        for userId in userIds do
            do! notifyUser env userId message
    }

// Infra dependencies
type ISmsEnv =
    abstract SmsClient: SmsClient
type IEmailEnv =
    abstract EmailClient: EmailClient
type IDbEnv =
    abstract DbClient: DbClient

type DataEnv = {
    SmsClient: SmsClient
    EmailClient: EmailClient
    DbClient: DbClient
} with
    interface ISmsClient with
        member this.SmsClient = this.SmsClient
    interface IEmailClient with
        member this.EmailClient = this.EmailClient
    interface IDbClient with
        member this.DbClient = this.DbClient

// Infra dependencies implementation
type OperationEnv(env: DataEnv) =
    interface IGetUserSettings with
        member this.GetUserSettings(userId) =
            DatabaseService.getUserSettings env userId
    interface ISendSms with
        member this.SendSms(address, message) =
            SmsService.sendSms env address message
    interface ISendEmail with
        member this.SendEmail(phone, message) =
            EmailService.sendEmail env number message

module ScenarioHandler = 
    let notifyUser dataEnv userId =
        let operationEnv = OperationEnv(dataEnv)
        BusinessHandler.notifyUser operationEnv userId