# eHourWPF
<div style="background-color: yellow; color: black; padding: 10px;">
    <h3>an almost legally binding contract? idk, just want to clear myself.</h3>
    <ul>
        <h4>Definitions ↓</h4>
        <li>Creator: npxrc/Neil Patrao/any other aliases of mine</li>
        <li>Consumer/Consumers: Anyone who clicks a download link to download either the npxrc/ehourwinapp or npxrc/ehourwpf application</li>
        <li><a href="https://www.olatheschools.org/site/handlers/filedownload.ashx?moduleinstanceid=35131&dataid=52172&FileName=OPS%20STUDENT%20ACCEPTABLE%20USE%20POLICY%202023-2024%20ENGLISH.pdf">AUP: Acceptable Use Policy, click to open.</a></li>
    </ul>
    <div style="background-color: black; height: 2px; width: 100%;"></div>
    <br>
    FOR LEGAL REASONS I am required (probably not, it just adds some spice) to inform you that:<br>
    <ul>
        <li>Any misuse of either npxrc/ehourwinapp or npxrc/ehourwpf application(s) or package(s) is the Consumer/Consumers fault, and is not the responsibility of the Creator, npxrc.</li>
        <li>The Creator (npxrc), is merely providing you a graphical interface similar to that of the Academy Endorsement Portal created by the Olathe School District. npxrc/ehourwinapp and npxrc/ehourwpf application(s) or package(s) are no different in terms of functionality compared to the Academy Endorsement Portal provided by the Olathe School District to high school students.</li>
        <li>What does that mean? I'm trying to say that if you can DDoS or congest the Olathe School District portal through their website, then it is possible through my App and cannot be fixed unless they add a rate limiter.</li>
    </ul>
</div>

<hr>

<div>
    <h1>This app is NOT a virus or malware, read below</h1>
    So you probably put the app into Windows Defender to see if it's a virus, which is fair. I'm just some nobody on GitHub making an app. Defender marks it as clear (at least as of 10:55 CST on 8/7/2024), but you're still sketched out, so you put it into VirusTotal, and find that Malwarebytes and MaxSecure flagged it as MachineLearning/Anomalous.100% and Trojan.Malware.300983.susgen. 
    <br>
    <img src="https://github.com/npxrc/ehourwpf/blob/master/README-Assets/notavirus.png?raw=true">
    <br>
    I want to tell you that both of those are NOT virus flags. Those are because MalwareBytes has never seen the file before (hence "Anomalous", meaning irregular), and MaxSecure is just flagging it heuristically, meaning that it thinks you should proceed with caution since it's not been seen before.
    <br>
    <br>
    And besides, why would <b><i>I</i></b> want to steal your credentials, or give you a virus, if the code is literally open source. Exactly. I don't even know how to give you a virus even if I wanted to since I can't code C#, have you even seen any of the code? It's a dumpster fire. So with that aside, you can read the rest of the README.md.
</div>
<br>

# actually useful info

So you might've seen my other repository <a href="https://github.com/npxrc/ehourwinapp">at npxrc/ehourwinapp on GitHub,</a> and are probably wondering 2 things:
- Why are you starting a new repo? And,
- Why haven't you added the most important feature, being able to submit eHour requests?

<br>Haha, good question. So anyways, this is a new project of mine, which relies on the WPF framework which is most certainly going out of business soon, and will be replaced with WinUI 3.
<br>So,
- why not create a WinUI app instead?
    - because I have no clue how to design an app from code!

Okay, fine. To answer your first question, as a design student I want to make my app look somewhat good. Let's be honest, if you look below and compare the two, which looks better? That's what I thought.
<br>
<br>![Old app.](https://github.com/npxrc/ehourwpf/blob/master/README-Assets/old.png?raw=true)![New app.](https://github.com/npxrc/ehourwpf/blob/master/README-Assets/new.png?raw=true)
<br>
To answer the other question, you have to understand I don't know how to code C#. [If you had read my README on my profile](https://github.com/npxrc/npxrc/README.md), you would've known that I actually code in JavaScript most of the time! In theory I should know how to make a eHour submission page, I mean ChatGPT generated a perfectly flawless script that I haven't tested yet.
<br>
<br>
That should answer all of your questions, but I guess I'll make a FAQ of questions that nobody has asked me yet.
<br>
# FAQ (nobody has ever asked me any of these)
- <b><u>Is any of this legal?</u></b>
    -  I hope so. Probably not illegal, but COULD violate ([make sure to read the AUP of the Olathe District](https://www.olatheschools.org/site/handlers/filedownload.ashx?moduleinstanceid=35131&dataid=52172&FileName=OPS%20STUDENT%20ACCEPTABLE%20USE%20POLICY%202023-2024%20ENGLISH.pdf)):
        - Section 1 Clause B (Under Student Rights & Responsibilities) since I'm dropping a diss on the eHours web portal SOON
        - Section 5 Subject 3 (Network Access (Major), idk what to call it) Clause 3, I'm not sure if a poorly created website is considered a restriction or not, so up to some people from the Tech Centre to decide.
- <b><u>Are you logging this for eHours?</u></b>
    - Absolutely, will be logging this for at least 100 eHours. And since it's my app, I can log more than 100. I have no clue why there's a limit, but it's silly.
- <b><u>Why do you want to make this?</u></b>
    - As a UI/UX designer (i'm literally not) and as someone who has used CSS before, I understand it is not that hard to style a website. I don't own a web server for myself and I don't *really* know how to code PHP (server processing language). But I do understand it's not hard to write CSS. A bit sad that the wayback machine only goes back to 2020 but it means I can roast the eHours website even more.
- <b><u>I want to get into designing in C# and WPF, how can I do that?</u></b>
    - No. 1: Don't, please do something useful with your life.
    - But if you want to, [then download Visual Studio](https://visualstudio.microsoft.com/downloads/), scroll to Desktop & Mobile after clicking Modify or whatever, and check .NET desktop, and optionally Windows Application Development (if you want to code C# for WinUI).
        - I prefer WPF but that's because I can use Blend for Visual Studio, which comes with a GUI.
            - If you just want to make a simple app, go with Windows Forms
            - If you want to make an app with a nice interface, go with Windows Presentation Foundation. A bit harder but looks prettier.
            - If you want to make a fully modern app that looks something like Apple Music or... any other Windows 11 app... then go with WinUI 3. Also download the gallery app from the microsoft store.
    - From there, then watch some YouTube tutorials or pull up your AI of choice (I like Claude) and Google.
- <b><u>Who are you?</u></b>
    - I am Batman. You could probably find out who I am through my GitHub icon, or through the "almost-legally-binding" contract of sorts at the top, which you SHOULD'VE read.